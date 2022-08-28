// <copyright file="UserBoundedHelpers.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using CSGOTunes.API.Spotify.Exceptions;
using CSGOTunes.API.Spotify.Interfaces;
using CSGOTunes.API.Users.Interfaces;
using CSGOTunes.API.Users.Models;

namespace CSGOTunes.API.Users.Helpers
{
    /// <summary>
    /// User-bounded helper methods.
    /// </summary>
    public static class UserBoundedHelpers
    {
        /// <summary>
        /// Perform a user-bounded operation, which will automatically refresh the access token if necessary.
        /// This method will pass-through any exceptions that are thrown by the operation.
        /// </summary>
        /// <param name="userModel">An instance of <see cref="UserModel"/>.</param>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        /// <param name="operation">The async user-bounded operation, which takes the access token as a parameter.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <typeparam name="TResult">The type of result that the user-bounded operation returns.</typeparam>
        /// <returns>An instance of <typeparamref name="TResult"/>.</returns>
        public static async Task<TResult> DoUserBoundedOperationAsync<TResult>(
            UserModel userModel,
            ISpotifyService spotifyService,
            IUserRepository userRepository,
            Func<string, Task<TResult>> operation,
            CancellationToken cancellationToken)
        {
            var effectiveExpiresAt = DateTimeOffset.FromUnixTimeMilliseconds(userModel.AccessTokenExpiresAt)
                .Subtract(TimeSpan.FromMinutes(5))
                .ToUnixTimeMilliseconds();

            if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() >= effectiveExpiresAt)
            {
                userModel = await PerformRefreshAndSaveNewTokenAsync(
                    userModel,
                    spotifyService,
                    userRepository,
                    cancellationToken);
            }

            try
            {
                var result = await operation(userModel.AccessToken);
                return result;
            }
            catch (SpotifyTokenExpiredException)
            {
                userModel = await PerformRefreshAndSaveNewTokenAsync(
                    userModel,
                    spotifyService,
                    userRepository,
                    cancellationToken);
                return await operation(userModel.AccessToken);
            }
        }

        /// <summary>
        /// Perform a user-bounded operation, which will automatically refresh the access token if necessary.
        /// This method will pass-through any exceptions that are thrown by the operation.
        /// </summary>
        /// <param name="userModel">An instance of <see cref="UserModel"/>.</param>
        /// <param name="spotifyService">An instance of <see cref="ISpotifyService"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        /// <param name="operation">The async user-bounded operation, which takes the access token as a parameter.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public static async Task DoUserBoundedOperationAsync(
            UserModel userModel,
            ISpotifyService spotifyService,
            IUserRepository userRepository,
            Func<string, Task> operation,
            CancellationToken cancellationToken)
        {
            await DoUserBoundedOperationAsync(
                userModel,
                spotifyService,
                userRepository,
                async accessToken =>
                {
                    await operation(accessToken);
                    return 0;
                },
                cancellationToken);
        }

        private static async Task<UserModel> PerformRefreshAndSaveNewTokenAsync(
            UserModel userModel,
            ISpotifyService spotifyService,
            IUserRepository userRepository,
            CancellationToken cancellationToken)
        {
            var refreshTokenResponse = await spotifyService.RefreshTokenAsync(
                userModel.RefreshToken,
                cancellationToken);

            var expiresIn = refreshTokenResponse.ExpiresIn ?? 0;
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn).ToUnixTimeMilliseconds();

            var updatedUserModel = userModel with
            {
                AccessToken = refreshTokenResponse.AccessToken ?? string.Empty,
                AccessTokenExpiresAt = expiresAt,
            };

            await userRepository.UpdateAsync(updatedUserModel, cancellationToken);
            return updatedUserModel;
        }
    }
}
