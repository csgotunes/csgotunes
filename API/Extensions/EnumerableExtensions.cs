// <copyright file="EnumerableExtensions.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace CSGOTunes.API.Extensions
{
    /// <summary>
    /// Extension methods for enumerable collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Get the non-null items from an enumerable and return them in a non-nullable annotated enumerable.
        /// </summary>
        /// <param name="self">An input enumerable to filter.</param>
        /// <typeparam name="T">The type of the item that the enumerable collection holds.</typeparam>
        /// <returns>A non-nullable version of the enumerable with all null items filtered out.</returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> self)
        {
            return self.Where(item => item != null)!;
        }
    }
}
