// <copyright file="PlayException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that the call to play has failed.
    /// </summary>
    public class PlayException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public PlayException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public PlayException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public PlayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
