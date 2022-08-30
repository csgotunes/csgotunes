// <copyright file="GameStateEventModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.GameState.Models
{
    /// <summary>
    /// A game-state integration event, which is queued and processed.
    /// </summary>
    public sealed record GameStateEventModel
    {
        /// <summary>
        /// Gets the Spotify user ID for the event.
        /// </summary>
        public string? SpotifyUserID { get; init; }

        /// <summary>
        /// Gets the CFG key for the event.
        /// </summary>
        public string? CFGKey { get; init; }

        /// <summary>
        /// Gets the game state request.
        /// </summary>
        public GameStateRequestModel? Request { get; init; }
    }
}
