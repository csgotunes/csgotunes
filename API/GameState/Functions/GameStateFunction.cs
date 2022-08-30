// <copyright file="GameStateFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.GameState.Models;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Users.Helpers;
using CSGOTunes.API.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CSGOTunes.API.GameState.Functions
{
    /// <summary>
    /// Processes game-state integration events.
    /// </summary>
    public sealed class GameStateFunction
    {
        private readonly ISpotifyService spotifyService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateFunction"/> class.
        /// </summary>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        public GameStateFunction(
            ISpotifyService spotifyService,
            IUserRepository userRepository)
        {
            this.spotifyService = spotifyService;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Run the function and complete authentication.
        /// </summary>
        /// <param name="httpRequest">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="NoContentResult"/>.</returns>
        [FunctionName(nameof(GameStateFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "game-state")]
            HttpRequest httpRequest,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var spotifyUserID = httpRequest.Query["spotifyUserID"].FirstOrDefault() ?? string.Empty;
            var cfgKey = httpRequest.Query["cfgKey"].FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(spotifyUserID) || string.IsNullOrWhiteSpace(cfgKey))
            {
                return new NotFoundResult();
            }

            var user = await this.userRepository.GetByIDAsync(spotifyUserID, cancellationToken);

            if (user == null || user.CFGKey != cfgKey || user.IsDisabled)
            {
                return new NotFoundResult();
            }

            user = user with
            {
                LastSeenAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };

            await this.userRepository.UpdateAsync(user, cancellationToken);

            var (gameStateRequest, modelState) = await httpRequest.ReadAndValidateAsync<GameStateRequestModel>(cancellationToken);

            if (gameStateRequest == null || !modelState.IsValid)
            {
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }

            // Ignore this event if we aren't getting something for the provider.
            if (gameStateRequest.Provider == null
                || string.IsNullOrWhiteSpace(gameStateRequest.Provider.SteamID)
                || gameStateRequest.Player?.SteamID == null
                || gameStateRequest.Player.State == null
                || !gameStateRequest.Player.SteamID.Equals(
                    gameStateRequest.Provider.SteamID,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return new NoContentResult();
            }

            var timestampMilliseconds = (long)gameStateRequest.Provider.Timestamp * 1000;
            var isLifeChangeNewerEvent = timestampMilliseconds >= user.AliveStateChangeTimestamp;

            var didJustDie = gameStateRequest.Player.State.Health <= 0
                             && user.IsAlive
                             && isLifeChangeNewerEvent;

            var didJustSpawn = gameStateRequest.Player.State.Health > 0
                               && !user.IsAlive
                               && isLifeChangeNewerEvent;

            // No need to waste cycles if there wasn't some sort of state change.
            if (!didJustDie && !didJustSpawn)
            {
                return new NoContentResult();
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
                return new NoContentResult();
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
            return new NoContentResult();
        }
    }
}
