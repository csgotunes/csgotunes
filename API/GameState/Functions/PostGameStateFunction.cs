// <copyright file="PostGameStateFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.GameState.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSGOTunes.API.GameState.Functions
{
    /// <summary>
    /// Processing incoming POST requests for game-state integration events.
    /// </summary>
    public sealed class PostGameStateFunction
    {
        private readonly QueueServiceClient queueServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostGameStateFunction"/> class.
        /// </summary>
        /// <param name="queueServiceClient">An instance of <see cref="QueueServiceClient"/>.</param>
        public PostGameStateFunction(QueueServiceClient queueServiceClient)
        {
            this.queueServiceClient = queueServiceClient;
        }

        /// <summary>
        /// Run the function and complete authentication.
        /// </summary>
        /// <param name="httpRequest">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="NoContentResult"/>.</returns>
        [FunctionName(nameof(PostGameStateFunction))]
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

            var (gameStateRequest, modelState) = await httpRequest.ReadAndValidateAsync<GameStateRequestModel>(cancellationToken);

            if (gameStateRequest == null || !modelState.IsValid)
            {
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }

            var queueClient = this.queueServiceClient.GetQueueClient("game-state");
            await queueClient.SendMessageAsync(
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GameStateEventModel
                {
                    SpotifyUserID = spotifyUserID,
                    CFGKey = cfgKey,
                    Request = gameStateRequest,
                }))),
                TimeSpan.Zero,
                cancellationToken: cancellationToken);

            return new NoContentResult();
        }
    }
}
