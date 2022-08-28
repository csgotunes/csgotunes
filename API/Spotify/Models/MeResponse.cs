// <copyright file="MeResponse.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Models
{
    /// <summary>
    /// A response from Spotify's get user endpoint.
    /// </summary>
    public record MeResponse
    {
        /// <summary>
        /// Gets the HREF for the user, which is a unique HTTPS link to the profile.
        /// </summary>
        [JsonProperty("href")]
        public string? HREF { get; init; }

        /// <summary>
        /// Gets the ID for the user, which is unique across the Spotify platform.
        /// </summary>
        [JsonProperty("id")]
        public string? ID { get; init; }

        /// <summary>
        /// Gets the type of the user, which should always be user.
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; init; }

        /// <summary>
        /// Gets the URI of the user.
        /// </summary>
        [JsonProperty("uri")]
        public string? URI { get; init; }
    }
}
