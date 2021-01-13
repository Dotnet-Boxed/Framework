namespace Boxed.Mapping
{
    using System;
    using System.Collections.Generic;
#if NET5_0
    using System.Collections.Immutable;
#endif
    using System.Collections.ObjectModel;
    using System.Linq;
#if NET5_0 || NETSTANDARD2_1
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// <see cref="IMapper{TSource, TDestination}"/> extension methods.
    /// </summary>
    public static class MapperExtensions
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
            this IMapper<TSource, TDestination> mapper,
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
                mapper.Map(sourceItem, destinationItem);
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
        /// <returns>The mapped object of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static TDestination Map<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource source)
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
            mapper.Map(source, destination);
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
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="sourceCollection"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] MapArray<TSourceCollection, TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSourceCollection sourceCollection,
            TDestination[] destinationCollection)
            where TSourceCollection : IEnumerable<TSource>
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            if (destinationCollection is null)
            {
                throw new ArgumentNullException(nameof(destinationCollection));
            }

            var i = 0;
            foreach (var item in sourceCollection)
            {
                var destination = Factory<TDestination>.CreateInstance();
                mapper.Map(item, destination);
                destinationCollection[i] = destination;
                ++i;
            }

            return destinationCollection;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] MapArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
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

            var destination = new TDestination[source.Count];
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination[i] = destinationItem;
            }

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
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] MapArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
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

            var destination = new TDestination[source.Count];
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination[i] = destinationItem;
            }

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
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] MapArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
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

            var destination = new TDestination[source.Length];
            for (var i = 0; i < source.Length; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination[i] = destinationItem;
            }

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
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] MapArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
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

            var destination = new TDestination[source.Count()];
            var i = 0;
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination[i] = destinationItem;
                ++i;
            }

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
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <returns>A collection of type <typeparamref name="TDestinationCollection"/> containing objects of type
        /// <typeparamref name="TDestination" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper" /> or <paramref name="sourceCollection" /> is
        /// <c>null</c>.</exception>
        public static TDestinationCollection MapCollection<TSourceCollection, TSource, TDestinationCollection, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSourceCollection sourceCollection,
            TDestinationCollection destinationCollection)
            where TSourceCollection : IEnumerable<TSource>
            where TDestinationCollection : ICollection<TDestination>
            where TDestination : new()
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            foreach (var item in sourceCollection)
            {
                var destination = Factory<TDestination>.CreateInstance();
                mapper.Map(item, destination);
                destinationCollection.Add(destination);
            }

            return destinationCollection;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Collection<TDestination> MapCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
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

            var destination = new Collection<TDestination>();
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Collection<TDestination> MapCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
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

            var destination = new Collection<TDestination>();
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Collection<TDestination> MapCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
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

            var destination = new Collection<TDestination>();
            for (var i = 0; i < source.Length; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Collection<TDestination> MapCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
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

            var destination = new Collection<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
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

            var destination = new HashSet<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
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

            var destination = new HashSet<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
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

            var destination = new HashSet<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into a hash set of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
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

            var destination = new HashSet<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }

#if NET5_0
        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into an immutable array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableArray<TDestination> MapImmutableArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
            where TDestination : new() =>
            ImmutableArray.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into an immutable array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableArray<TDestination> MapImmutableArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
            where TDestination : new() =>
            ImmutableArray.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into an immutable array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableArray<TDestination> MapImmutableArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
            where TDestination : new() =>
            ImmutableArray.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into an immutable array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableArray<TDestination> MapImmutableArray<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            ImmutableArray.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into an immutable list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableList<TDestination> MapImmutableList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
            where TDestination : new() =>
            ImmutableList.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the collection of <typeparamref name="TSource"/> into an immutable list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableList<TDestination> MapImmutableList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
            where TDestination : new() =>
            ImmutableList.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the array of <typeparamref name="TSource"/> into an immutable list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableList<TDestination> MapImmutableList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
            where TDestination : new() =>
            ImmutableList.Create(mapper.MapArray(source));

        /// <summary>
        /// Maps the enumerable of <typeparamref name="TSource"/> into an immutable list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An immutable list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ImmutableList<TDestination> MapImmutableList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            ImmutableList.Create(mapper.MapArray(source));
#endif

        /// <summary>
        /// Maps the list of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static List<TDestination> MapList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
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

            var destination = new List<TDestination>(source.Count);
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static List<TDestination> MapList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
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

            var destination = new List<TDestination>(source.Count);
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static List<TDestination> MapList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
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

            var destination = new List<TDestination>(source.Length);
            for (var i = 0; i < source.Length; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static List<TDestination> MapList<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
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

            var destination = new List<TDestination>(source.Count());
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

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
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ObservableCollection<TDestination> MapObservableCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            List<TSource> source)
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

            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ObservableCollection<TDestination> MapObservableCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            Collection<TSource> source)
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

            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < source.Count; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ObservableCollection<TDestination> MapObservableCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            TSource[] source)
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

            var destination = new ObservableCollection<TDestination>();
            for (var i = 0; i < source.Length; ++i)
            {
                var sourceItem = source[i];
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Insert(i, destinationItem);
            }

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
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static ObservableCollection<TDestination> MapObservableCollection<TSource, TDestination>(
            this IMapper<TSource, TDestination> mapper,
            IEnumerable<TSource> source)
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

            var destination = new ObservableCollection<TDestination>();
            foreach (var sourceItem in source)
            {
                var destinationItem = Factory<TDestination>.CreateInstance();
                mapper.Map(sourceItem, destinationItem);
                destination.Add(destinationItem);
            }

            return destination;
        }
    }
}
