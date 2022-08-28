// <copyright file="CompleteAuthFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Auth.Models;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.Nonces.Interfaces;
using CSGOTunes.API.Sessions.Interfaces;
using CSGOTunes.API.Sessions.Models;
using CSGOTunes.API.Spotify.Exceptions;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Users.Interfaces;
using CSGOTunes.API.Users.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CSGOTunes.API.Auth.Functions
{
    /// <summary>
    /// Complete the auth-flow for logging into Spotify.
    /// </summary>
    public sealed class CompleteAuthFunction
    {
        private readonly INonceRepository nonceRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly ISpotifyService spotifyService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteAuthFunction"/> class.
        /// </summary>
        /// <param name="nonceRepository">An instance of <see cref="INonceRepository"/>.</param>
        /// <param name="sessionRepository">An instance of <see cref="ISessionRepository"/>.</param>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        public CompleteAuthFunction(
            INonceRepository nonceRepository,
            ISessionRepository sessionRepository,
            ISpotifyService spotifyService,
            IUserRepository userRepository)
        {
            this.nonceRepository = nonceRepository;
            this.sessionRepository = sessionRepository;
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
        [FunctionName(nameof(CompleteAuthFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "complete-auth")]
            HttpRequest httpRequest,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var (completeAuthRequestModel, modelState) = await httpRequest.ReadAndValidateAsync<CompleteAuthRequestModel>(cancellationToken);

            if (completeAuthRequestModel == null || !modelState.IsValid)
            {
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }

            var nonce = await this.nonceRepository.GetByIDAsync(completeAuthRequestModel.State ?? string.Empty, cancellationToken);

            // You get one shot at completing auth for a particular nonce.
            if (nonce != null)
            {
                await this.nonceRepository.DeleteAsync(nonce.ID, cancellationToken);
            }

            if (nonce == null || DateTimeOffset.FromUnixTimeMilliseconds(nonce.ExpiresAt) <= DateTimeOffset.UtcNow)
            {
                modelState.AddModelError(string.Empty, "Your nonce has expired.");
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }

            try
            {
                var exchangeTokenResponse = await this.spotifyService.ExchangeTokenAsync(
                    completeAuthRequestModel.Code ?? string.Empty,
                    cancellationToken);

                var accessToken = exchangeTokenResponse.AccessToken ?? string.Empty;
                var refreshToken = exchangeTokenResponse.RefreshToken ?? string.Empty;
                var expiresIn = exchangeTokenResponse.ExpiresIn ?? 0;
                var expiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn).ToUnixTimeMilliseconds();

                var spotifyProfile = await this.spotifyService.GetMeAsync(
                    accessToken,
                    cancellationToken);

                var userID = spotifyProfile.ID ?? string.Empty;

                var existingUser = await this.userRepository.GetByIDAsync(userID, cancellationToken);

                if (existingUser != null)
                {
                    var updatedUser = existingUser with
                    {
                        AccessToken = accessToken,
                        AccessTokenExpiresAt = expiresAt,
                        RefreshToken = refreshToken,
                    };

                    await this.userRepository.UpdateAsync(updatedUser, cancellationToken);
                }
                else
                {
                    var newUser = new UserModel(
                        userID,
                        accessToken,
                        expiresAt,
                        refreshToken,
                        Guid.NewGuid().ToString("N"),
                        0,
                        false,
                        false,
                        DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                    await this.userRepository.AddAsync(newUser, cancellationToken);
                }

                var sessionID = Guid.NewGuid().ToString("N");

                await this.sessionRepository.AddAsync(
                    new SessionModel(sessionID, userID),
                    cancellationToken);

                return new OkObjectResult(new CompleteAuthResponseModel(sessionID));
            }
            catch (Exception exception) when (exception is ExchangeTokenException or GetMeException)
            {
                modelState.AddModelError(string.Empty, "Unable to authenticate.");
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }
        }
    }
}
