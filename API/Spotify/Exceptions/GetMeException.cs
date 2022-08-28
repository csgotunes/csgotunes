// <copyright file="GetMeException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that the call to retrieve the user's Spotify profile failed.
    /// </summary>
    public class GetMeException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public GetMeException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public GetMeException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public GetMeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
