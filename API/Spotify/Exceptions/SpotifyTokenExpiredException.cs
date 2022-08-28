// <copyright file="SpotifyTokenExpiredException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that the access token has expired.
    /// </summary>
    public class SpotifyTokenExpiredException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public SpotifyTokenExpiredException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SpotifyTokenExpiredException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SpotifyTokenExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
