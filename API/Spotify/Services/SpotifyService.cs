// <copyright file="SpotifyService.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Configuration;
using CSGOTunes.API.Spotify.Exceptions;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Spotify.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CSGOTunes.API.Spotify.Services
{
    /// <inheritdoc cref="ISpotifyService"/>
    public sealed class SpotifyService : ISpotifyService
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<CSGOTunesConfiguration> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyService"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/>.</param>
        /// <param name="configuration">An instance of <see cref="IOptions{CSGOTunesConfiguration}"/>.</param>
        public SpotifyService(
            HttpClient httpClient,
            IOptions<CSGOTunesConfiguration> configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task<ExchangeTokenResponse> ExchangeTokenAsync(
            string code,
            CancellationToken cancellationToken)
        {
            var redirectURI = this.configuration.Value.SpotifyRedirectURI;

            if (redirectURI == null)
            {
                throw new InvalidOperationException("The Spotify redirect URI is not properly configured for the application.");
            }

            var request = new HttpRequestMessage
            {
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "code", code },
                        { "redirect_uri", redirectURI.ToString() },
                        { "grant_type", "authorization_code" },
                    }),
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                Headers =
                {
                    Authorization = this.GetClientCredentialsAuthHeader(),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new ExchangeTokenException("Unable to exchange token.", ex);
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<ExchangeTokenResponse>(responseString);

            if (response == null)
            {
                throw new ExchangeTokenException("Unable to exchange token.");
            }

            return response;
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task<MeResponse> GetMeAsync(
            string accessToken,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.spotify.com/v1/me"),
                Headers =
                {
                    Authorization = GetBearerAuthHeader(accessToken),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SpotifyTokenExpiredException("The access token is bad or has expired.");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is not SpotifyTokenExpiredException)
            {
                throw new GetMeException("Unable to get user's profile.", ex);
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<MeResponse>(responseString);

            if (response == null)
            {
                throw new GetMeException("Unable to get user's profile.");
            }

            return response;
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task<PlaybackStateResponse?> GetPlayerAsync(
            string accessToken,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.spotify.com/v1/me/player"),
                Headers =
                {
                    Authorization = GetBearerAuthHeader(accessToken),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SpotifyTokenExpiredException("The access token is bad or has expired.");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is not SpotifyTokenExpiredException)
            {
                throw new GetMeException("Unable to get user's playback state.", ex);
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<PlaybackStateResponse>(responseString);

            if (response == null)
            {
                throw new GetMeException("Unable to get user's playback state.");
            }

            return response;
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task PlayAsync(
            string accessToken,
            string playbackDeviceID,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("https://api.spotify.com/v1/me/player/play"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(new PlayRequest
                    {
                        DeviceID = playbackDeviceID,
                    }),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json),
                Headers =
                {
                    Authorization = GetBearerAuthHeader(accessToken),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            try
            {
                HttpResponseMessage httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SpotifyTokenExpiredException("The access token is bad or has expired.");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is not SpotifyTokenExpiredException)
            {
                throw new PlayException("Unable to play the provided device ID.", ex);
            }
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task PauseAsync(
            string accessToken,
            string playbackDeviceID,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("https://api.spotify.com/v1/me/player/pause"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(new PauseRequest
                    {
                        DeviceID = playbackDeviceID,
                    }),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json),
                Headers =
                {
                    Authorization = GetBearerAuthHeader(accessToken),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            try
            {
                HttpResponseMessage httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SpotifyTokenExpiredException("The access token is bad or has expired.");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex) when (ex is not SpotifyTokenExpiredException)
            {
                throw new PauseException("Unable to pause the provided device ID.", ex);
            }
        }

        /// <inheritdoc cref="ISpotifyService"/>
        public async Task<RefreshTokenResponse> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken)
        {
            var redirectURI = this.configuration.Value.SpotifyRedirectURI;

            if (redirectURI == null)
            {
                throw new InvalidOperationException("The Spotify redirect URI is not properly configured for the application.");
            }

            var request = new HttpRequestMessage
            {
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", refreshToken },
                    }),
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                Headers =
                {
                    Authorization = this.GetClientCredentialsAuthHeader(),
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json),
                    },
                },
            };

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await this.httpClient.SendAsync(request, cancellationToken);
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new RefreshTokenException("Unable to refresh token.", ex);
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseString);

            if (response == null)
            {
                throw new RefreshTokenException("Unable to refresh token.");
            }

            return response;
        }

        private static AuthenticationHeaderValue GetBearerAuthHeader(string accessToken)
        {
            return new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private AuthenticationHeaderValue GetClientCredentialsAuthHeader()
        {
            var authenticationString = $"{this.configuration.Value.SpotifyClientID}:{this.configuration.Value.SpotifyClientSecret}";
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));

            return new AuthenticationHeaderValue("Basic", encodedString);
        }
    }
}
