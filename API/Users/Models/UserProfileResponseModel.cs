// <copyright file="UserProfileResponseModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Users.Models
{
    /// <summary>
    /// A user's profile which contains their CS:GO Tunes settings.
    /// </summary>
    /// <param name="SpotifyUserID">The user's Spotify user ID.</param>
    /// <param name="CFGKey">The user's current CFG key.</param>
    /// <param name="LastSeenAt">The last time that the user was seen pinging the game-state endpoint.</param>
    /// <param name="IsDisabled">A value indicating whether CS:GO Tunes is disabled for the user.</param>
    public sealed record UserProfileResponseModel(
        string SpotifyUserID,
        string CFGKey,
        long LastSeenAt,
        bool IsDisabled);
}
