// <copyright file="ProviderRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.GameState.Models
{
    /// <summary>
    /// Part of the game-state request payload representing the provider, aka the person/machine playing the game.
    /// </summary>
    public sealed record ProviderRequestModel
    {
        /// <summary>
        /// Gets the Steam ID for the provider, who is the person actually playing the game.
        /// </summary>
        public string? SteamID { get; init; }

        /// <summary>
        /// Gets the current timestamp (in seconds) for the provider.
        /// </summary>
        public int Timestamp { get; init; }
    }
}
