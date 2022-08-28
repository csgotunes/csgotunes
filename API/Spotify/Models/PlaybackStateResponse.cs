// <copyright file="PlaybackStateResponse.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Models
{
    /// <summary>
    /// A response from Spotify's get user playback state.
    /// </summary>
    public record PlaybackStateResponse
    {
        /// <summary>
        /// Gets the playback device for this state.
        /// </summary>
        [JsonProperty("device")]
        public PlaybackDeviceResponse? Device { get; init; }

        /// <summary>
        /// Gets a value indicating whether there is currently something playing.
        /// </summary>
        [JsonProperty("is_playing")]
        public bool IsPlaying { get; init; }
    }
}
