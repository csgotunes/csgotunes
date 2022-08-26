// <copyright file="NonceModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Nonces.Models
{
    /// <summary>
    /// A nonce which represents an API-initiate flow for authentication.
    /// The ID is passed as the `state` for the OAuth flow to prevent request forgery.
    /// </summary>
    public sealed record NonceModel(
        string ID,
        long ExpiresAt);
}
