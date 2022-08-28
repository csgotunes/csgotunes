// <copyright file="InitAuthFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Auth.Models;
using CSGOTunes.API.Configuration;
using CSGOTunes.API.Nonces.Interfaces;
using CSGOTunes.API.Nonces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSGOTunes.API.Auth.Functions
{
    /// <summary>
    /// Initiate the auth-flow for logging into Spotify.
    /// </summary>
    public sealed class InitAuthFunction
    {
        private readonly IOptions<CSGOTunesConfiguration> configuration;
        private readonly INonceRepository nonceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitAuthFunction"/> class.
        /// </summary>
        /// <param name="configuration">An instance of <see cref="IOptions{CSGOTunesConfiguration}"/>.</param>
        /// <param name="nonceRepository">An instance of <see cref="INonceRepository"/>.</param>
        public InitAuthFunction(
            IOptions<CSGOTunesConfiguration> configuration,
            INonceRepository nonceRepository)
        {
            this.configuration = configuration;
            this.nonceRepository = nonceRepository;
        }

        /// <summary>
        /// Run the function and initiate authentication.
        /// </summary>
        /// <param name="req">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="NoContentResult"/>.</returns>
        [FunctionName(nameof(InitAuthFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "init-auth")]
            HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeMilliseconds();

            var nonce = new NonceModel(
                Guid.NewGuid().ToString("N"),
                expiresAt);

            await this.nonceRepository.AddAsync(nonce, cancellationToken);

            return new OkObjectResult(new InitAuthResponseModel(this.BuildSpotifyLoginUrl(nonce)));
        }

        private Uri BuildSpotifyLoginUrl(NonceModel nonceModel)
        {
            var clientID = this.configuration.Value.SpotifyClientID;

            if (string.IsNullOrWhiteSpace(clientID))
            {
                throw new InvalidOperationException("The Spotify client ID is not properly configured for the application.");
            }

            var redirectURI = this.configuration.Value.SpotifyRedirectURI;

            if (redirectURI == null)
            {
                throw new InvalidOperationException("The Spotify redirect URI is not properly configured for the application.");
            }

            const string scope = "user-read-private user-read-email user-modify-playback-state user-read-playback-state";

            return new Uri("https://accounts.spotify.com/authorize?response_type=code"
                           + "&client_id=" + Uri.EscapeDataString(clientID)
                           + "&scope=" + Uri.EscapeDataString(scope)
                           + "&redirect_uri=" + Uri.EscapeDataString(redirectURI.ToString())
                           + "&state=" + Uri.EscapeDataString(nonceModel.ID));
        }
    }
}
