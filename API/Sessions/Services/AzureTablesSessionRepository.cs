// <copyright file="AzureTablesSessionRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.Sessions.Exceptions;
using CSGOTunes.API.Sessions.Interfaces;
using CSGOTunes.API.Sessions.Models;

namespace CSGOTunes.API.Sessions.Services
{
    /// <summary>
    /// A session repository implementation that utilities Azure Storage Tables.
    /// </summary>
    /// <inheritdoc cref="ISessionRepository"/>
    public sealed class AzureTablesSessionRepository : ISessionRepository
    {
        private readonly TableClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTablesSessionRepository"/> class.
        /// </summary>
        /// <param name="client">An instance of <see cref="TableServiceClient"/>.</param>
        public AzureTablesSessionRepository(TableServiceClient client)
        {
            this.client = client.GetTableClient("sessions");
        }

        /// <inheritdoc cref="ISessionRepository"/>
        public async Task<SessionModel?> GetByIDAsync(
            string sessionID,
            CancellationToken cancellationToken)
        {
            var existingSessionEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                sessionID,
                sessionID,
                cancellationToken: cancellationToken);

            return existingSessionEntity == null ? null : FromTableEntity(existingSessionEntity);
        }

        /// <inheritdoc cref="ISessionRepository"/>
        public async Task AddAsync(
            SessionModel sessionModel,
            CancellationToken cancellationToken)
        {
            var existingSessionEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                sessionModel.ID,
                sessionModel.ID,
                null,
                cancellationToken);

            if (existingSessionEntity != null)
            {
                throw new SessionAlreadyExistsException($"The session ID {sessionModel.ID} already exists.");
            }

            await this.client.AddEntityAsync(
                ToTableEntity(sessionModel),
                cancellationToken);
        }

        /// <inheritdoc cref="ISessionRepository"/>
        public async Task DeleteAsync(
            string sessionID,
            CancellationToken cancellationToken)
        {
            var existingSessionEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                sessionID,
                sessionID,
                cancellationToken: cancellationToken);

            if (existingSessionEntity == null)
            {
                return;
            }

            await this.client.DeleteEntityAsync(
                sessionID,
                sessionID,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc cref="ISessionRepository"/>
        public async Task UpdateAsync(SessionModel sessionModel, CancellationToken cancellationToken)
        {
            var existingSessionEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                sessionModel.ID,
                sessionModel.ID,
                null,
                cancellationToken);

            if (existingSessionEntity == null)
            {
                throw new SessionDoesNotExistException($"The session ID {sessionModel.ID} does not exist.");
            }

            await this.client.UpdateEntityAsync(
                ToTableEntity(sessionModel),
                ETag.All,
                TableUpdateMode.Replace,
                cancellationToken);
        }

        private static SessionModel FromTableEntity(TableEntity tableEntity)
        {
            return new SessionModel(
                tableEntity.RowKey,
                tableEntity.GetString(nameof(SessionModel.SpotifyUserID)));
        }

        private static TableEntity ToTableEntity(SessionModel sessionModel)
        {
            return new TableEntity(
                sessionModel.ID,
                sessionModel.ID)
            {
                { nameof(SessionModel.SpotifyUserID), sessionModel.SpotifyUserID },
            };
        }
    }
}
