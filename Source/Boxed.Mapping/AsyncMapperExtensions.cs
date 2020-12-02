namespace Boxed.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="IAsyncMapper{TSource, TDestination}"/> extension methods.
    /// </summary>
    public static class AsyncMapperExtensions
    {
#if NET5_0 || NETSTANDARD2_1
        /// <summary>
        /// Maps the <see cref="IAsyncEnumerable{TSource}"/> into <see cref="IAsyncEnumerable{TDestination}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source asynchronous enumerable.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="IAsyncEnumerable{TDestination}"/> collection.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async IAsyncEnumerable<TDestination> MapEnumerableAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IAsyncEnumerable<TSource> source,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            await foreach (var sourceItem in source.ConfigureAwait(false).WithCancellation(cancellationToken))
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                await mapper.MapAsync(sourceItem, destinationItem, cancellationToken).ConfigureAwait(false);
                yield return destinationItem;
            }
        }
#endif

        /// <summary>
        /// Maps the specified source object to a new object with a type of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDestination">The type of the destination object.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source object.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The mapped object of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination> MapAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = Factory<TDestination>.CreateInstance();
            await mapper.MapAsync(source, destination, cancellationToken).ConfigureAwait(false);
            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source collection.</param>
        /// <param name="destination">The destination collection.</param>
        /// <param name="sourceCount">The number of items in the source collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> MapArrayAsync<TSourceCollection, TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSourceCollection source,
            TDestination[] destination,
            int? sourceCount = null,
            CancellationToken cancellationToken = default)
            where TSourceCollection : IEnumerable<TSource>
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var tasks = new Task[sourceCount ?? source.Count()];
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination[i] = destinationItem;
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);

                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> MapArrayAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            List<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new TDestination[sourceCount];
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination[i] = destinationItem;
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> MapArrayAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            Collection<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new TDestination[sourceCount];
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination[i] = destinationItem;
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> MapArrayAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource[] source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Length;
            var tasks = new Task[sourceCount];
            var destination = new TDestination[sourceCount];
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination[i] = destinationItem;
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> MapArrayAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var destination = new TDestination[sourceCount];
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination[i] = destinationItem;
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource" /> into a collection of type
        /// <typeparamref name="TDestinationCollection" /> containing objects of type <typeparamref name="TDestination" />.
        /// </summary>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestinationCollection">The type of the destination collection.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source collection.</param>
        /// <param name="destination">The destination collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of type <typeparamref name="TDestinationCollection"/> containing objects of type
        /// <typeparamref name="TDestination" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static async Task<TDestinationCollection> MapCollectionAsync<TSourceCollection, TSource, TDestinationCollection, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSourceCollection source,
            TDestinationCollection destination,
            CancellationToken cancellationToken = default)
            where TSourceCollection : IEnumerable<TSource>
            where TDestinationCollection : ICollection<TDestination>
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Add(destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<Collection<TDestination>> MapCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            List<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new Collection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<Collection<TDestination>> MapCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            Collection<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new Collection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<Collection<TDestination>> MapCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource[] source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Length;
            var tasks = new Task[sourceCount];
            var destination = new Collection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<Collection<TDestination>> MapCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var destination = new Collection<TDestination>();
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<HashSet<TDestination>> MapHashSetAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            List<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new HashSet<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Add(destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<HashSet<TDestination>> MapHashSetAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            Collection<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new HashSet<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Add(destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<HashSet<TDestination>> MapHashSetAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource[] source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Length;
            var tasks = new Task[sourceCount];
            var destination = new HashSet<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Add(destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<HashSet<TDestination>> MapHashSetAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var destination = new HashSet<TDestination>();
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Add(destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<List<TDestination>> MapListAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            List<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new List<TDestination>(sourceCount);
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<List<TDestination>> MapListAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            Collection<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new List<TDestination>(sourceCount);
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<List<TDestination>> MapListAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource[] source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Length;
            var tasks = new Task[sourceCount];
            var destination = new List<TDestination>(sourceCount);
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<List<TDestination>> MapListAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var destination = new List<TDestination>(sourceCount);
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<ObservableCollection<TDestination>> MapObservableCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            List<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<ObservableCollection<TDestination>> MapObservableCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            Collection<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count;
            var tasks = new Task[sourceCount];
            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<ObservableCollection<TDestination>> MapObservableCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            TSource[] source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Length;
            var tasks = new Task[sourceCount];
            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < sourceCount; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<ObservableCollection<TDestination>> MapObservableCollectionAsync<TSource, TDestination>(
            this IAsyncMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default)
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sourceCount = source.Count();
            var tasks = new Task[sourceCount];
            var destination = new ObservableCollection<TDestination>();
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                destination.Insert(i, destinationItem);
                tasks[i] = mapper.MapAsync(sourceItem, destinationItem, cancellationToken);
                ++i;
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return destination;
        }
    }
}
