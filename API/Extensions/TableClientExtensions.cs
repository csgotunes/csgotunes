// <copyright file="TableClientExtensions.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace CSGOTunes.API.Extensions
{
    /// <summary>
    /// Extension methods for Azure Storage's table client.
    /// </summary>
    public static class TableClientExtensions
    {
        /// <summary>
        /// Get an entity by partition key and row key, returning null instead of throwing an exception if the entity
        /// is not found.
        /// </summary>
        /// <param name="self">The table client to fetch the entity from.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="select">The fields to select for the entity.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <typeparam name="T">The type for the entity.</typeparam>
        /// <returns>An instance of the entity or null if not found.</returns>
        public static async Task<T?> GetEntityOrNullAsync<T>(
            this TableClient self,
            string partitionKey,
            string rowKey,
            IEnumerable<string>? select = null,
            CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            try
            {
                var response = await self.GetEntityAsync<T>(
                    partitionKey,
                    rowKey,
                    select,
                    cancellationToken);

                return response?.Value;
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == (int)HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
