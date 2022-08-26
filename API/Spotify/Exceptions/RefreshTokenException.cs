// <copyright file="RefreshTokenException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that a token refresh failed.
    /// </summary>
    public class RefreshTokenException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public RefreshTokenException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public RefreshTokenException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public RefreshTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
