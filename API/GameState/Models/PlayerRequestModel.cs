// <copyright file="PlayerRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.GameState.Models
{
    /// <summary>
    /// Part of the game-state request payload representing the player that is being observed.
    /// This is not necessarily the person whose machine it is.
    /// </summary>
    public sealed record PlayerRequestModel
    {
        /// <summary>
        /// Gets the Steam ID for the observed player.
        /// </summary>
        public string? SteamID { get; init; }

        /// <summary>
        /// Gets the state for the observed player.
        /// </summary>
        public PlayerStateRequestModel? State { get; init; }
    }
}
