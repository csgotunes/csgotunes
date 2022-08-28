// <copyright file="CompleteAuthRequestModel.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace CSGOTunes.API.Auth.Models
{
    /// <summary>
    /// The request  for completing authentication.
    /// </summary>
    public sealed record CompleteAuthRequestModel
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        [Required]
        public string? Code { get; init; }

        /// <summary>
        /// Gets the OAuth state.
        /// </summary>
        [Required]
        public string? State { get; init; }
    }
}
