// <copyright file="SessionDoesNotExistException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Sessions.Exceptions
{
    /// <summary>
    /// An exception indicating that a session does not exist.
    /// </summary>
    public class SessionDoesNotExistException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public SessionDoesNotExistException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SessionDoesNotExistException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public SessionDoesNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
