// <copyright file="Startup.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CSGOTunes.API.Startup))]

namespace CSGOTunes.API
{
    /// <summary>
    /// Initializes the dependency injection container for the functions application.
    /// </summary>
    public sealed class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configure the dependency injection container.
        /// </summary>
        /// <param name="builder">An instance of <see cref="IFunctionsHostBuilder"/>.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
