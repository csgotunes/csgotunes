// <copyright file="CompleteAuthResponseModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

namespace CSGOTunes.API.Auth.Models
{
    /// <summary>
    /// The response for completing authentication.
    /// </summary>
    /// <param name="SessionID">The session ID.</param>
    public sealed record CompleteAuthResponseModel(string SessionID);
}
