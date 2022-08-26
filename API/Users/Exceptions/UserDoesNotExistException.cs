// <copyright file="UserDoesNotExistException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Users.Exceptions
{
    /// <summary>
    /// An exception indicating that a user does not exist.
    /// </summary>
    public class UserDoesNotExistException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public UserDoesNotExistException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public UserDoesNotExistException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public UserDoesNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
