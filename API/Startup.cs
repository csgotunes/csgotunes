// <copyright file="Startup.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using CSGOTunes.API.Configuration;
using CSGOTunes.API.Nonces.Interfaces;
using CSGOTunes.API.Nonces.Services;
using CSGOTunes.API.Sessions.Interfaces;
using CSGOTunes.API.Sessions.Services;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Spotify.Services;
using CSGOTunes.API.Users.Interfaces;
using CSGOTunes.API.Users.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

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
            builder.Services
                .AddHttpClient<ISpotifyService, SpotifyService>()
                .AddPolicyHandler((_, _) => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(2)));

            // Set up the Azure SDK clients
            builder.Services.AddAzureClients(clientFactoryBuilder =>
            {
                var azureConnectionString = builder.GetContext().Configuration["AzureWebJobsStorage"];
                clientFactoryBuilder.AddTableServiceClient(azureConnectionString);
                clientFactoryBuilder.AddQueueServiceClient(azureConnectionString);
            });

            // Register our repositories
            builder.Services.AddScoped<INonceRepository, AzureTablesNonceRepository>();
            builder.Services.AddScoped<ISessionRepository, AzureTablesSessionRepository>();
            builder.Services.AddScoped<IUserRepository, AzureTablesUserRepository>();

            // For more information, see: https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#working-with-options-and-settings
            builder.Services.AddOptions<CSGOTunesConfiguration>()
                .Configure<IConfiguration>((settings, configuration)
                    => configuration.GetSection("CSGOTunes").Bind(settings));
        }
    }
}
