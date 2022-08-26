// <copyright file="AuthHelpers.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Sessions.Interfaces;
using CSGOTunes.API.Sessions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CSGOTunes.API.Auth.Helpers
{
    /// <summary>
    /// Helpers related to auth.
    /// </summary>
    public static class AuthHelpers
    {
        /// <summary>
        /// Auth an incoming HTTP-request with a session-bounded bearer token.
        /// </summary>
        /// <param name="httpRequest">An instance of <see cref="HttpRequest"/>.</param>
        /// <param name="sessionRepository">An instance of <see cref="ISessionRepository"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="SessionModel"/> or null if auth was not successful.</returns>
        public static async Task<SessionModel?> AuthAsync(
            HttpRequest httpRequest,
            ISessionRepository sessionRepository,
            CancellationToken cancellationToken)
        {
            if (!httpRequest.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return null;
            }

            var headerValue = httpRequest.Headers[HeaderNames.Authorization].ToString();
            var headerParsed = AuthenticationHeaderValue.TryParse(headerValue, out var authenticationHeaderValue);

            if (!headerParsed || authenticationHeaderValue == null)
            {
                return null;
            }

            if (authenticationHeaderValue.Scheme != "Bearer")
            {
                return null;
            }

            var sessionID = authenticationHeaderValue.Parameter?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(sessionID))
            {
                return null;
            }

            return await sessionRepository.GetByIDAsync(sessionID, cancellationToken);
        }
    }
}
