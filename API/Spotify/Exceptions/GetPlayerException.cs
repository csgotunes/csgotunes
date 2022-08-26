// <copyright file="GetPlayerException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that the call to retrieve the user's Spotify playback state failed.
    /// </summary>
    public class GetPlayerException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public GetPlayerException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public GetPlayerException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public GetPlayerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
