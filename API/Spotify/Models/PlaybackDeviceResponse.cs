// <copyright file="PlaybackDeviceResponse.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Models
{
    /// <summary>
    /// A device response represented within the get user playback state endpoint.
    /// </summary>
    public record PlaybackDeviceResponse
    {
        /// <summary>
        /// Gets the ID of the playback device.
        /// </summary>
        [JsonProperty("id")]
        public string? ID { get; init; }

        /// <summary>
        /// Gets a value indicating whether the player is currently active.
        /// </summary>
        [JsonProperty("is_active")]
        public bool IsActive { get; init; }

        /// <summary>
        /// Gets a value indicating whether the player is currently restricted, which means you can not control the playback.
        /// </summary>
        [JsonProperty("is_restricted")]
        public bool IsRestricted { get; init; }
    }
}
