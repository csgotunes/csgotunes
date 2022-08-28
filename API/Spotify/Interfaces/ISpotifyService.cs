// <copyright file="ISpotifyService.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Spotify.Exceptions;
using CSGOTunes.API.Spotify.Models;

namespace CSGOTunes.API.Spotify.Interfaces
{
    /// <summary>
    /// A repository for communicating with Spotify.
    /// </summary>
    public interface ISpotifyService
    {
        /// <summary>
        /// Exchange an auth-code for a set of tokens.
        /// </summary>
        /// <param name="code">The code to exchange for tokens.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A response containing the access token and refresh token from the exchange.</returns>
        /// <exception cref="ExchangeTokenException">If the token exchange fails.</exception>
        Task<ExchangeTokenResponse> ExchangeTokenAsync(
            string code,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the currently logged-in user's profile.
        /// </summary>
        /// <param name="accessToken">The access token for the user.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="MeResponse"/>.</returns>
        /// <exception cref="SpotifyTokenExpiredException">If the access token has expired or is bad.</exception>
        /// <exception cref="GetMeException">If the endpoint returns an error.</exception>
        Task<MeResponse> GetMeAsync(
            string accessToken,
            CancellationToken cancellationToken);

        /// <summary>
        /// Exchange a refresh token for a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A response containing the access token from the refresh.</returns>
        /// <exception cref="RefreshTokenException">If the token refresh fails.</exception>
        Task<RefreshTokenResponse> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the Spotify user's playback state.
        /// </summary>
        /// <param name="accessToken">The access token for the user.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="PlaybackStateResponse"/>.</returns>
        /// <exception cref="SpotifyTokenExpiredException">If the access token has expired or is bad.</exception>
        /// <exception cref="GetPlayerException">If the endpoint returns an error.</exception>
        Task<PlaybackStateResponse> GetPlayerAsync(
            string accessToken,
            CancellationToken cancellationToken);

        /// <summary>
        /// Play/resume the Spotify user's playback.
        /// </summary>
        /// <param name="accessToken">The access token for the user.</param>
        /// <param name="playbackDeviceID">The device ID to play/resume.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="SpotifyTokenExpiredException">If the access token has expired or is bad.</exception>
        /// <exception cref="PlayException">If the endpoint returns an error.</exception>
        Task PlayAsync(
            string accessToken,
            string playbackDeviceID,
            CancellationToken cancellationToken);

        /// <summary>
        /// Pause the Spotify user's playback.
        /// </summary>
        /// <param name="accessToken">The access token for the user.</param>
        /// <param name="playbackDeviceID">The device ID to paue.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        /// <exception cref="SpotifyTokenExpiredException">If the access token has expired or is bad.</exception>
        /// <exception cref="PauseException">If the endpoint returns an error.</exception>
        Task PauseAsync(
            string accessToken,
            string playbackDeviceID,
            CancellationToken cancellationToken);
    }
}
