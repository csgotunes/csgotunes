// <copyright file="AzureTablesUserRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.Users.Exceptions;
using CSGOTunes.API.Users.Interfaces;
using CSGOTunes.API.Users.Models;

namespace CSGOTunes.API.Users.Services
{
    /// <summary>
    /// A user repository implementation that utilities Azure Storage Tables.
    /// </summary>
    /// <inheritdoc cref="IUserRepository"/>
    public sealed class AzureTablesUserRepository : IUserRepository
    {
        private readonly TableClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTablesUserRepository"/> class.
        /// </summary>
        /// <param name="client">An instance of <see cref="TableServiceClient"/>.</param>
        public AzureTablesUserRepository(TableServiceClient client)
        {
            this.client = client.GetTableClient("users");
        }

        /// <inheritdoc cref="IUserRepository"/>
        public async Task<UserModel?> GetByIDAsync(
            string spotifyUserID,
            CancellationToken cancellationToken)
        {
            var existingUserEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                spotifyUserID,
                spotifyUserID,
                cancellationToken: cancellationToken);

            return existingUserEntity == null ? null : FromTableEntity(existingUserEntity);
        }

        /// <inheritdoc cref="IUserRepository"/>
        public async Task AddAsync(
            UserModel userModel,
            CancellationToken cancellationToken)
        {
            var existingUserEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                userModel.SpotifyUserID,
                userModel.SpotifyUserID,
                null,
                cancellationToken);

            if (existingUserEntity != null)
            {
                throw new UserAlreadyExistsException($"The user with ID {userModel.SpotifyUserID} already exists.");
            }

            await this.client.AddEntityAsync(
                ToTableEntity(userModel),
                cancellationToken);
        }

        /// <inheritdoc cref="IUserRepository"/>
        public async Task DeleteAsync(
            string spotifyUserID,
            CancellationToken cancellationToken)
        {
            var existingUserEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                spotifyUserID,
                spotifyUserID,
                cancellationToken: cancellationToken);

            if (existingUserEntity == null)
            {
                return;
            }

            await this.client.DeleteEntityAsync(
                spotifyUserID,
                spotifyUserID,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc cref="IUserRepository"/>
        public async Task UpdateAsync(UserModel userModel, CancellationToken cancellationToken)
        {
            var existingUserEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                userModel.SpotifyUserID,
                userModel.SpotifyUserID,
                null,
                cancellationToken);

            if (existingUserEntity == null)
            {
                throw new UserDoesNotExistException($"The user with ID {userModel.SpotifyUserID} does not exist.");
            }

            await this.client.UpdateEntityAsync(
                ToTableEntity(userModel),
                ETag.All,
                TableUpdateMode.Replace,
                cancellationToken);
        }

        private static UserModel FromTableEntity(TableEntity tableEntity)
        {
            return new UserModel(
                tableEntity.RowKey,
                tableEntity.GetString(nameof(UserModel.AccessToken)) ?? string.Empty,
                tableEntity.GetInt64(nameof(UserModel.AccessTokenExpiresAt)) ?? 0L,
                tableEntity.GetString(nameof(UserModel.RefreshToken)) ?? string.Empty,
                tableEntity.GetString(nameof(UserModel.CFGKey)) ?? string.Empty,
                tableEntity.GetInt64(nameof(UserModel.LastSeenAt)) ?? 0L,
                tableEntity.GetBoolean(nameof(UserModel.IsDisabled)) ?? false,
                tableEntity.GetBoolean(nameof(UserModel.IsAlive)) ?? false,
                tableEntity.GetInt64(nameof(UserModel.AliveStateChangeTimestamp)) ?? 0L);
        }

        private static TableEntity ToTableEntity(UserModel userModel)
        {
            return new TableEntity(
                userModel.SpotifyUserID,
                userModel.SpotifyUserID)
            {
                { nameof(UserModel.AccessToken), userModel.AccessToken },
                { nameof(UserModel.AccessTokenExpiresAt), userModel.AccessTokenExpiresAt },
                { nameof(UserModel.RefreshToken), userModel.RefreshToken },
                { nameof(UserModel.CFGKey), userModel.CFGKey },
                { nameof(UserModel.LastSeenAt), userModel.LastSeenAt },
                { nameof(UserModel.IsDisabled), userModel.IsDisabled },
                { nameof(UserModel.IsAlive), userModel.IsAlive },
                { nameof(UserModel.AliveStateChangeTimestamp), userModel.AliveStateChangeTimestamp },
            };
        }
    }
}
