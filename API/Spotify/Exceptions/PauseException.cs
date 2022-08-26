// <copyright file="PauseException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that the call to pause has failed.
    /// </summary>
    public class PauseException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public PauseException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public PauseException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public PauseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
