// <copyright file="PlayRequest.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Models
{
    /// <summary>
    /// A request to Spotify to play/resume on a specific device ID.
    /// </summary>
    public sealed record PlayRequest
    {
        /// <summary>
        /// Gets the device ID.
        /// </summary>
        [JsonProperty("device_id")]
        public string? DeviceID { get; init; }
    }
}
