// <copyright file="UserAlreadyExistsException.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Users.Exceptions
{
    /// <summary>
    /// An exception indicating that a user already exists.
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        public UserAlreadyExistsException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        public UserAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
