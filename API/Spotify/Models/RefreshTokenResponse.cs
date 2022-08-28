// <copyright file="RefreshTokenResponse.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Models
{
    /// <summary>
    /// A response from Spotify's refresh token endpoint.
    /// For more information, see: https://developer.spotify.com/documentation/general/guides/authorization/code-flow/.
    /// </summary>
    public sealed record RefreshTokenResponse
    {
        /// <summary>
        /// Gets the access token returned by the refresh.
        /// </summary>
        [JsonProperty("access_token")]
        public string? AccessToken { get; init; }

        /// <summary>
        /// Gets the token type returned by the refresh.
        /// </summary>
        [JsonProperty("token_type")]
        public string? TokenType { get; init; }

        /// <summary>
        /// Gets the scopes granted by the refresh.
        /// </summary>
        [JsonProperty("scope")]
        public string? Scope { get; init; }

        /// <summary>
        /// Gets the number of seconds until the access token expires.
        /// </summary>
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; init; }
    }
}
