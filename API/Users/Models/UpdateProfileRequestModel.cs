// <copyright file="UpdateProfileRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Users.Models
{
    /// <summary>
    /// The request  for updating user profile settings.
    /// </summary>
    public sealed record UpdateProfileRequestModel
    {
        /// <summary>
        /// Gets a value indicating whether CS:GO Tunes is disabled for the user.
        /// </summary>
        public bool IsDisabled { get; init; }
    }
}
