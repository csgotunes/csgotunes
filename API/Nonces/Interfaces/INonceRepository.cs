// <copyright file="INonceRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Nonces.Exceptions;
using CSGOTunes.API.Nonces.Models;

namespace CSGOTunes.API.Nonces.Interfaces
{
    /// <summary>
    /// A repository for managing nonces.
    /// </summary>
    public interface INonceRepository
    {
        /// <summary>
        /// Get a nonce by ID.
        /// </summary>
        /// <param name="nonceID">The ID of the nonce.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>The nonce model, or null if not found.</returns>
        Task<NonceModel?> GetByIDAsync(string nonceID, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new nonce.
        /// </summary>
        /// <param name="nonceModel">The nonce to add.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="NonceAlreadyExistsException">If the nonce ID is already in use.</exception>
        Task AddAsync(NonceModel nonceModel, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a nonce.
        /// </summary>
        /// <param name="nonceID">The ID of the nonce to delete.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="NonceDoesNotExistException">If the nonce ID does not exist.</exception>
        Task DeleteAsync(string nonceID, CancellationToken cancellationToken);
    }
}
