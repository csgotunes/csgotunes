// <copyright file="GameStateRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.GameState.Models
{
    /// <summary>
    /// A game-state integration request model payload, which gives information about the player's game.
    /// </summary>
    public sealed record GameStateRequestModel
    {
        /// <summary>
        /// Gets the provider for the game-state request.
        /// </summary>
        public ProviderRequestModel? Provider { get; init; }

        /// <summary>
        /// Gets the player information for the game-state request, which represents the observed player.
        /// </summary>
        public PlayerRequestModel? Player { get; init; }
    }
}
