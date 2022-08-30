// <copyright file="ProcessGameStateFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.GameState.Models;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Users.Helpers;
using CSGOTunes.API.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSGOTunes.API.GameState.Functions
{
    /// <summary>
    /// Processes game-state integration events.
    /// </summary>
    public sealed class ProcessGameStateFunction
    {
        private readonly ISpotifyService spotifyService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessGameStateFunction"/> class.
        /// </summary>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        public ProcessGameStateFunction(
            ISpotifyService spotifyService,
            IUserRepository userRepository)
        {
            this.spotifyService = spotifyService;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Run the function and process the game state event.
        /// </summary>
        /// <param name="queueItem">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="NoContentResult"/>.</returns>
        [FunctionName(nameof(ProcessGameStateFunction))]
        public async Task RunAsync(
            [QueueTrigger("game-state")]
            string queueItem,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(queueItem))
            {
                return;
            }

            var gameStateEvent = JsonConvert.DeserializeObject<GameStateEventModel>(queueItem);

            if (gameStateEvent == null || gameStateEvent.Request == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(gameStateEvent.SpotifyUserID) || string.IsNullOrWhiteSpace(gameStateEvent.CFGKey))
            {
                return;
            }

            var user = await this.userRepository.GetByIDAsync(gameStateEvent.SpotifyUserID, cancellationToken);

            if (user == null || user.CFGKey != gameStateEvent.CFGKey || user.IsDisabled)
            {
                return;
            }

            user = user with
            {
                LastSeenAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };

            await this.userRepository.UpdateAsync(user, cancellationToken);

            // Ignore this event if we aren't getting something for the provider.
            if (gameStateEvent.Request.Provider == null
                || string.IsNullOrWhiteSpace(gameStateEvent.Request.Provider.SteamID)
                || gameStateEvent.Request.Player?.SteamID == null
                || gameStateEvent.Request.Player.State == null
                || !gameStateEvent.Request.Player.SteamID.Equals(
                    gameStateEvent.Request.Provider.SteamID,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var timestampMilliseconds = (long)gameStateEvent.Request.Provider.Timestamp * 1000;
            var isLifeChangeNewerEvent = timestampMilliseconds >= user.AliveStateChangeTimestamp;

            var didJustDie = gameStateEvent.Request.Player.State.Health <= 0
                             && user.IsAlive
                             && isLifeChangeNewerEvent;

            var didJustSpawn = gameStateEvent.Request.Player.State.Health > 0
                               && !user.IsAlive
                               && isLifeChangeNewerEvent;

            // No need to waste cycles if there wasn't some sort of state change.
            if (!didJustDie && !didJustSpawn)
            {
                return;
            }

            var playbackStateResponse = await UserBoundedHelpers.DoUserBoundedOperationAsync(
                user,
                this.spotifyService,
                this.userRepository,
                async accessToken => await this.spotifyService.GetPlayerAsync(accessToken, cancellationToken),
                cancellationToken);

            // We can't do anything if there is no playback device or if it is restricted (ie: blocking web API calls).
            if (playbackStateResponse == null
                || playbackStateResponse.Device == null
                || string.IsNullOrWhiteSpace(playbackStateResponse.Device.ID)
                || playbackStateResponse.Device.IsRestricted)
            {
                return;
            }

            if (didJustDie && !playbackStateResponse.IsPlaying)
            {
                await UserBoundedHelpers.DoUserBoundedOperationAsync(
                    user,
                    this.spotifyService,
                    this.userRepository,
                    async accessToken => await this.spotifyService.PlayAsync(
                        accessToken,
                        playbackStateResponse.Device.ID,
                        cancellationToken),
                    cancellationToken);
            }

            if (didJustSpawn && playbackStateResponse.IsPlaying)
            {
                await UserBoundedHelpers.DoUserBoundedOperationAsync(
                    user,
                    this.spotifyService,
                    this.userRepository,
                    async accessToken => await this.spotifyService.PauseAsync(
                        accessToken,
                        playbackStateResponse.Device.ID,
                        cancellationToken),
                    cancellationToken);
            }

            user = user with
            {
                IsAlive = didJustSpawn,
                AliveStateChangeTimestamp = timestampMilliseconds,
            };

            await this.userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
