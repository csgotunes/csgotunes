// <copyright file="ISessionRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Sessions.Exceptions;
using CSGOTunes.API.Sessions.Models;

namespace CSGOTunes.API.Sessions.Interfaces
{
    /// <summary>
    /// A repository for managing sessions.
    /// </summary>
    public interface ISessionRepository
    {
        /// <summary>
        /// Get a session by ID.
        /// </summary>
        /// <param name="sessionID">The ID of the session.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>The session model, or null if not found.</returns>
        Task<SessionModel?> GetByIDAsync(string sessionID, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new session.
        /// </summary>
        /// <param name="sessionModel">The session to add.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="SessionAlreadyExistsException">If the session ID is already in use.</exception>
        Task AddAsync(SessionModel sessionModel, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a session.
        /// </summary>
        /// <param name="sessionID">The ID of the session to delete.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="SessionDoesNotExistException">If the session ID does not exist.</exception>
        Task DeleteAsync(string sessionID, CancellationToken cancellationToken);
    }
}
