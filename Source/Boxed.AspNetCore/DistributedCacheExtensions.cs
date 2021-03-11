namespace Boxed.AspNetCore
{
    using System;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// <see cref="IDistributedCache"/> extension methods.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Gets the value of type <typeparamref name="T" /> with the specified key from the cache asynchronously by
        /// deserializing it from JSON format or returns <c>default(T)</c> if the key was not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="jsonSerializerOptions">The JSON serializer options or <c>null</c> to use the default.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is
        /// <c>null</c>.</exception>
        public static async Task<T?> GetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            JsonSerializerOptions? jsonSerializerOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var bytes = await cache.GetAsync(key, cancellationToken).ConfigureAwait(false);
            return Deserialize<T>(bytes, jsonSerializerOptions);
        }

        /// <summary>
        /// Sets the value of type <typeparamref name="T" /> with the specified key in the cache asynchronously by
        /// serializing it to JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <param name="jsonSerializerOptions">The JSON serializer options or <c>null</c> to use the default.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value of type <typeparamref name="T" /> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is
        /// <c>null</c>.</exception>
        public static Task SetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            DistributedCacheEntryOptions? options = null,
            JsonSerializerOptions? jsonSerializerOptions = null,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, jsonSerializerOptions);
            return cache.SetAsync(key, bytes, options, cancellationToken);
        }

        private static T? Deserialize<T>(byte[] bytes, JsonSerializerOptions? jsonSerializerOptions)
        {
            var utf8JsonReader = new Utf8JsonReader(bytes);
            return JsonSerializer.Deserialize<T>(ref utf8JsonReader, jsonSerializerOptions);
        }
    }
}
