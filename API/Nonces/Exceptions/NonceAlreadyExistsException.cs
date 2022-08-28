// <copyright file="NonceAlreadyExistsException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Nonces.Exceptions
{
    /// <summary>
    /// An exception indicating that a nonce already exists.
    /// </summary>
    public class NonceAlreadyExistsException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public NonceAlreadyExistsException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public NonceAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public NonceAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
