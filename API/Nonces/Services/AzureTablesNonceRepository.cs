// <copyright file="AzureTablesNonceRepository.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CSGOTunes.API.Extensions;
using CSGOTunes.API.Nonces.Exceptions;
using CSGOTunes.API.Nonces.Interfaces;
using CSGOTunes.API.Nonces.Models;

namespace CSGOTunes.API.Nonces.Services
{
    /// <summary>
    /// A nonce repository implementation that utilities Azure Storage Tables.
    /// </summary>
    /// <inheritdoc cref="INonceRepository"/>
    public sealed class AzureTablesNonceRepository : INonceRepository
    {
        private readonly TableClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTablesNonceRepository"/> class.
        /// </summary>
        /// <param name="client">An instance of <see cref="TableServiceClient"/>.</param>
        public AzureTablesNonceRepository(TableServiceClient client)
        {
            this.client = client.GetTableClient("nonces");
        }

        /// <inheritdoc cref="INonceRepository"/>
        public async Task<NonceModel?> GetByIDAsync(
            string nonceID,
            CancellationToken cancellationToken)
        {
            var existingNonceEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                nonceID,
                nonceID,
                cancellationToken: cancellationToken);

            return existingNonceEntity == null ? null : FromTableEntity(existingNonceEntity);
        }

        /// <inheritdoc cref="INonceRepository"/>
        public async Task AddAsync(
            NonceModel nonceModel,
            CancellationToken cancellationToken)
        {
            var existingNonceEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                nonceModel.ID,
                nonceModel.ID,
                null,
                cancellationToken);

            if (existingNonceEntity != null)
            {
                throw new NonceAlreadyExistsException($"The nonce ID {nonceModel.ID} already exists.");
            }

            await this.client.AddEntityAsync(
                ToTableEntity(nonceModel),
                cancellationToken);
        }

        /// <inheritdoc cref="INonceRepository"/>
        public async Task DeleteAsync(
            string nonceID,
            CancellationToken cancellationToken)
        {
            var existingNonceEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                nonceID,
                nonceID,
                cancellationToken: cancellationToken);

            if (existingNonceEntity == null)
            {
                return;
            }

            await this.client.DeleteEntityAsync(
                nonceID,
                nonceID,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc cref="INonceRepository"/>
        public async Task UpdateAsync(NonceModel nonceModel, CancellationToken cancellationToken)
        {
            var existingNonceEntity = await this.client.GetEntityOrNullAsync<TableEntity>(
                nonceModel.ID,
                nonceModel.ID,
                null,
                cancellationToken);

            if (existingNonceEntity == null)
            {
                throw new NonceDoesNotExistException($"The nonce ID {nonceModel.ID} does not exist.");
            }

            await this.client.UpdateEntityAsync(
                ToTableEntity(nonceModel),
                ETag.All,
                TableUpdateMode.Replace,
                cancellationToken);
        }

        private static NonceModel FromTableEntity(TableEntity tableEntity)
        {
            return new NonceModel(
                tableEntity.RowKey,
                tableEntity.GetInt64("ExpiresAt") ?? 0L);
        }

        private static TableEntity ToTableEntity(NonceModel nonceModel)
        {
            return new TableEntity(
                nonceModel.ID,
                nonceModel.ID)
            {
                { "ExpiresAt", nonceModel.ExpiresAt },
            };
        }
    }
}
