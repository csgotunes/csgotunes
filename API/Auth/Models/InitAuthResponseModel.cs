// <copyright file="InitAuthResponseModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;

namespace CSGOTunes.API.Auth.Models
{
    /// <summary>
    /// The response model from the initiate authentication endpoint, which contains a login URL.
    /// </summary>
    /// <param name="LoginUrl">The login URL that the client should proceed to.</param>
    public record InitAuthResponseModel(Uri LoginUrl);
}
