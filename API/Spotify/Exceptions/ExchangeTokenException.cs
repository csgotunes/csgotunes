// <copyright file="ExchangeTokenException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Spotify.Exceptions
{
    /// <summary>
    /// An exception indicating that a token exchange failed.
    /// </summary>
    public class ExchangeTokenException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public ExchangeTokenException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public ExchangeTokenException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public ExchangeTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
