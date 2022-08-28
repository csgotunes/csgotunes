// <copyright file="SessionModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Sessions.Models
{
    /// <summary>
    /// A session whose ID serves as a token to authenticate the related Spotify user ID.
    /// </summary>
    public sealed record SessionModel(
        string ID,
        string SpotifyUserID);
}
