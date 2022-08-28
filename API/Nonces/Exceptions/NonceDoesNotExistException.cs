// <copyright file="NonceDoesNotExistException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Nonces.Exceptions
{
    /// <summary>
    /// An exception indicating that a nonce does not exist.
    /// </summary>
    public class NonceDoesNotExistException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public NonceDoesNotExistException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public NonceDoesNotExistException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public NonceDoesNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
