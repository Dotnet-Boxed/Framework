namespace Boxed.AspNetCore;

using System;
using System.Buffers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// <see cref="IDistributedCache"/> extension methods.
/// </summary>
/// <summary>
/// Provides a simple convenience wrapper around <see cref="IDistributedCache"/>; note that this implementation
/// does not attempt to avoid problems with multiple callers all invoking the "get" method at once when
/// data becomes evicted for cache ("stampeding"), or any other concerns such as returning stale data while
/// refresh occurs in the background - these are future considerations for the cache implementation.
/// </summary>
/// <remarks>The overloads taking <c>TState</c> are useful when used with <c>static</c> get methods, to avoid
/// "capture" overheads, but in most everyday scenarios, it may be more convenient to use the simpler stateless
/// version.</remarks>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Gets a value from cache, with a caller-supplied <paramref name="getMethod" /> (async, stateless) that is used if the value is not yet available.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="key">The cache item key.</param>
    /// <param name="getMethod">The method used to retrieve the item.</param>
    /// <param name="options">The cache options.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The cached value.</returns>
    public static ValueTask<T> GetAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<CancellationToken, ValueTask<T>> getMethod,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellation = default) =>
        GetAsyncSharedAsync<int, T>(cache, key, state: 0, getMethod, options, cancellation); // use dummy state

    /// <summary>
    /// Gets a value from cache, with a caller-supplied <paramref name="getMethod"/> (sync, stateless) that is used if the value is not yet available.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="key">The cache item key.</param>
    /// <param name="getMethod">The method used to retrieve the item.</param>
    /// <param name="options">The cache options.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The cached value.</returns>
    public static ValueTask<T> GetAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<T> getMethod,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellation = default) =>
        GetAsyncSharedAsync<int, T>(cache, key, state: 0, getMethod, options, cancellation); // use dummy state

    /// <summary>
    /// Gets a value from cache, with a caller-supplied <paramref name="getMethod" /> (async, stateful) that is used if the value is not yet available.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="key">The cache item key.</param>
    /// <param name="state">The state.</param>
    /// <param name="getMethod">The method used to retrieve the item.</param>
    /// <param name="options">The cache options.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>
    /// The cached value.
    /// </returns>
    public static ValueTask<T> GetAsync<TState, T>(
        this IDistributedCache cache,
        string key,
        TState state,
        Func<TState, CancellationToken, ValueTask<T>> getMethod,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellation = default) =>
        GetAsyncSharedAsync<TState, T>(cache, key, state, getMethod, options, cancellation);

    /// <summary>
    /// Gets a value from cache, with a caller-supplied <paramref name="getMethod"/> (sync, stateful) that is used if the value is not yet available.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="key">The cache item key.</param>
    /// <param name="state">The state.</param>
    /// <param name="getMethod">The method used to retrieve the item.</param>
    /// <param name="options">The cache options.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>
    /// The cached value.
    /// </returns>
    public static ValueTask<T> GetAsync<TState, T>(
        this IDistributedCache cache,
        string key,
        TState state,
        Func<TState, T> getMethod,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellation = default) =>
        GetAsyncSharedAsync<TState, T>(cache, key, state, getMethod, options, cancellation);

    /// <summary>
    /// Provides a common implementation for the public-facing API, to avoid duplication.
    /// </summary>
    private static ValueTask<T> GetAsyncSharedAsync<TState, T>(
        IDistributedCache cache,
        string key,
        TState state,
        Delegate getMethod,
        DistributedCacheEntryOptions? options,
        CancellationToken cancellation)
    {
        ArgumentNullException.ThrowIfNull(cache);

        var pending = cache.GetAsync(key, cancellation);
        if (!pending.IsCompletedSuccessfully)
        {
            // async-result was not available immediately; go full-async
            return Awaited(cache, key, pending, state, getMethod, options, cancellation);
        }

        // GetAwaiter().GetResult() here is *not* "sync-over-async" - we've already
        // validated that this data was available synchronously, and we're eliding
        // the state machine overheads in the (hopefully high-hit-rate) success case
#pragma warning disable VSTHRD103 // Call async methods when in an async method
        var bytes = pending.GetAwaiter().GetResult();
#pragma warning restore VSTHRD103 // Call async methods when in an async method
        if (bytes is null)
        {
            // async-result was available but data is missing; go async for everything else
            return Awaited(cache, key, null, state, getMethod, options, cancellation);
        }

        // data was available synchronously; deserialize
        return new(Deserialize<T>(bytes));

        static async ValueTask<T> Awaited(
            IDistributedCache cache, // the underlying cache
            string key, // the key on the cache
            Task<byte[]?>? pending, // incomplete "get bytes" operation, if any
            TState state, // state possibly used by the get-method
            Delegate getMethod, // the get-method supplied by the caller
            DistributedCacheEntryOptions? options, // cache expiration, etc
            CancellationToken cancellation)
        {
            byte[]? bytes;
            if (pending is not null)
            {
                bytes = await pending.ConfigureAwait(false);
                if (bytes is not null)
                {
                    // data was available asynchronously
                    return Deserialize<T>(bytes);
                }
            }

            var result = getMethod switch
            {
                // we expect 4 use-cases; sync/async, with/without state
                Func<TState, CancellationToken, ValueTask<T>> get => await get(state, cancellation).ConfigureAwait(false),
                Func<TState, T> get => get(state),
                Func<CancellationToken, ValueTask<T>> get => await get(cancellation).ConfigureAwait(false),
                Func<T> get => get(),
                _ => throw new ArgumentException("Unexpected get method found.", nameof(getMethod)),
            };
            bytes = Serialize(result);

            if (options is null)
            {
                // not recommended; cache expiration should be considered
                // important, usually
                await cache.SetAsync(key, bytes, cancellation).ConfigureAwait(false);
            }
            else
            {
                await cache.SetAsync(key, bytes, options, cancellation).ConfigureAwait(false);
            }

            return result;
        }
    }

    // The current cache API is byte[]-based, but a wide range of
    // serializer choices are possible; here we use the inbuilt
    // System.Text.Json.JsonSerializer, which is a fair compromise
    // between being easy to configure and use on general types,
    // versus raw performance. Alternative (non-byte[]) storage
    // mechanisms are under consideration.
    //
    // If it is likely that you will change serializers during
    // upgrades (and you are using out-of-process storage), then
    // you may wish to use a sentinel prefix before the payload,
    // to allow you to safely switch between serializers;
    // alternatively, you may choose to use a key-prefix so that
    // the old data is simply not found (and expires naturally)
    private static T Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes)!;

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }
}
