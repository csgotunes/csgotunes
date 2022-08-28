// <copyright file="GetUserProfileFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Auth.Helpers;
using CSGOTunes.API.Sessions.Interfaces;
using CSGOTunes.API.Spotify.Exceptions;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Users.Helpers;
using CSGOTunes.API.Users.Interfaces;
using CSGOTunes.API.Users.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CSGOTunes.API.Users.Functions
{
    /// <summary>
    /// Get the currently logged-in user's profile.
    /// </summary>
    public sealed class GetUserProfileFunction
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ISpotifyService spotifyService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserProfileFunction"/> class.
        /// </summary>
        /// <param name="sessionRepository">An instance of <see cref="ISessionRepository"/>.</param>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        public GetUserProfileFunction(
            ISessionRepository sessionRepository,
            ISpotifyService spotifyService,
            IUserRepository userRepository)
        {
            this.sessionRepository = sessionRepository;
            this.spotifyService = spotifyService;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Run the function and get the user's profile.
        /// </summary>
        /// <param name="httpRequest">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="IActionResult"/>.</returns>
        [FunctionName(nameof(GetUserProfileFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user-profile")]
            HttpRequest httpRequest,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var session = await AuthHelpers.AuthAsync(
                httpRequest,
                this.sessionRepository,
                cancellationToken);

            if (session == null)
            {
                return new UnauthorizedResult();
            }

            var user = await this.userRepository.GetByIDAsync(session.SpotifyUserID, cancellationToken);

            if (user == null)
            {
                return new UnauthorizedResult();
            }

            // Verify their connection to Spotify is still good.
            try
            {
                await UserBoundedHelpers.DoUserBoundedOperationAsync(
                    user,
                    this.spotifyService,
                    this.userRepository,
                    async accessToken => await this.spotifyService.GetMeAsync(accessToken, cancellationToken),
                    cancellationToken);
            }
            catch (Exception ex) when (ex is SpotifyTokenExpiredException or GetMeException)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(new UserProfileResponseModel(
                user.SpotifyUserID,
                user.CFGKey,
                user.LastSeenAt,
                user.IsDisabled));
        }
    }
}
