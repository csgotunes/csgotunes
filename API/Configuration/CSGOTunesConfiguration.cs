// <copyright file="CSGOTunesConfiguration.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Configuration
{
    /// <summary>
    /// The configuration for the CS:GO Tunes application.
    /// </summary>
    public sealed class CSGOTunesConfiguration
    {
        /// <summary>
        /// Gets or sets the Spotify OAuth client ID.
        /// </summary>
        public string? SpotifyClientID { get; set; }

        /// <summary>
        /// Gets or sets the Spotify OAuth client secret.
        /// </summary>
        public string? SpotifyClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Spotify OAuth redirect URI.
        /// </summary>
        public Uri? SpotifyRedirectURI { get; set; }
    }
}
