// <copyright file="IUserRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Users.Exceptions;
using CSGOTunes.API.Users.Models;

namespace CSGOTunes.API.Users.Interfaces
{
    /// <summary>
    /// A repository for managing users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get a user by Spotify user ID.
        /// </summary>
        /// <param name="spotifyUserID">The Spotify ID of the user.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>The user model, or null if not found.</returns>
        Task<UserModel?> GetByIDAsync(string spotifyUserID, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="userModel">The user to add.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="UserAlreadyExistsException">If the user ID is already in use.</exception>
        Task AddAsync(UserModel userModel, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="spotifyUserID">The Spotify ID of the user to delete.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="UserDoesNotExistException">If the user ID does not exist.</exception>
        Task DeleteAsync(string spotifyUserID, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a new user.
        /// </summary>
        /// <param name="userModel">The user to update.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="UserDoesNotExistException">If the user ID does not exist.</exception>
        Task UpdateAsync(UserModel userModel, CancellationToken cancellationToken);
    }
}
