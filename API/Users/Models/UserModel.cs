// <copyright file="UserModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Users.Models
{
    /// <summary>
    /// A user with their configuration and Spotify tokens.
    /// </summary>
    public sealed record UserModel(
        string SpotifyUserID,
        string AccessToken,
        long AccessTokenExpiresAt,
        string RefreshToken,
        string CFGKey,
        long LastSeenAt,
        bool IsDisabled,
        bool IsAlive,
        long AliveStateChangeTimestamp);
}
