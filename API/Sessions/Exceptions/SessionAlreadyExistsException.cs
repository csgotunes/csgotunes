// <copyright file="SessionAlreadyExistsException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Sessions.Exceptions
{
    /// <summary>
    /// An exception indicating that a session already exists.
    /// </summary>
    public class SessionAlreadyExistsException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public SessionAlreadyExistsException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SessionAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SessionAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
