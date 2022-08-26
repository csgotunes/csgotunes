// <copyright file="PlayerStateRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.GameState.Models
{
    /// <summary>
    /// Part of the game-state request payload representing the state (aka health, armor, etc) of the player that is being observed.
    /// </summary>
    public sealed record PlayerStateRequestModel
    {
        /// <summary>
        /// Gets the health for the observed player.
        /// </summary>
        public int Health { get; init; }
    }
}
