// <copyright file="UpdateUserProfileFunction.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Auth.Helpers;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.Sessions.Interfaces;
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
    /// Update the currently logged-in user's profile.
    /// </summary>
    public sealed class UpdateUserProfileFunction
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserProfileFunction"/> class.
        /// </summary>
        /// <param name="sessionRepository">An instance of <see cref="ISessionRepository"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        public UpdateUserProfileFunction(
            ISessionRepository sessionRepository,
            IUserRepository userRepository)
        {
            this.sessionRepository = sessionRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Run the function and update the user's profile.
        /// </summary>
        /// <param name="httpRequest">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="log">An instance of <see cref="ILogger"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="IActionResult"/>.</returns>
        [FunctionName(nameof(UpdateUserProfileFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user-profile")]
            HttpRequest httpRequest,
            ILogger log,
            CancellationToken cancellationToken)
        {
            var (updateProfileRequest, modelState) = await httpRequest.ReadAndValidateAsync<UpdateProfileRequestModel>(cancellationToken);

            if (updateProfileRequest == null || !modelState.IsValid)
            {
                return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
            }

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

            var updatedUser = user with
            {
                IsDisabled = updateProfileRequest.IsDisabled,
            };

            await this.userRepository.UpdateAsync(updatedUser, cancellationToken);

            return new NoContentResult();
        }
    }
}
