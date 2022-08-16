namespace Boxed.Mapping;

using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using System.Collections.Immutable;
#endif
using System.Collections.ObjectModel;
using System.Linq;
#if NET5_0_OR_GREATER || NETSTANDARD2_1
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#endif

/// <summary>
/// <see cref="IImmutableMapper{TSource, TDestination}"/> extension methods.
/// </summary>
public static class ImmutableMapperExtensions
{
#if NET5_0_OR_GREATER || NETSTANDARD2_1
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IAsyncEnumerable<TSource> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        await foreach (var sourceItem in source.ConfigureAwait(false).WithCancellation(cancellationToken))
        {
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = mapper.Map(source);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSourceCollection sourceCollection,
        TDestination[] destinationCollection)
        where TSourceCollection : IEnumerable<TSource>
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sourceCollection);
        ArgumentNullException.ThrowIfNull(destinationCollection);
#else
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
#endif

        var i = 0;
        foreach (var item in sourceCollection)
        {
            var destination = mapper.Map(item);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new TDestination[source.Count];
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new TDestination[source.Count];
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new TDestination[source.Length];
        for (var i = 0; i < source.Length; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

#if NET6_0_OR_GREATER
        if (!source.TryGetNonEnumeratedCount(out var count))
        {
            count = source.Count();
        }

        var destination = new TDestination[count];
#else
        var destination = new TDestination[source.Count()];
#endif
        var i = 0;
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSourceCollection sourceCollection,
        TDestinationCollection destinationCollection)
        where TSourceCollection : IEnumerable<TSource>
        where TDestinationCollection : ICollection<TDestination>
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sourceCollection);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (sourceCollection is null)
        {
            throw new ArgumentNullException(nameof(sourceCollection));
        }
#endif

        foreach (var item in sourceCollection)
        {
            var destination = mapper.Map(item);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new Collection<TDestination>();
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new Collection<TDestination>();
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new Collection<TDestination>();
        for (var i = 0; i < source.Length; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new Collection<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }

    /// <summary>
    /// Maps the list of <typeparamref name="TSource"/> into a hash set of
    /// <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
    /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
    /// <param name="mapper">The mapper.</param>
    /// <param name="source">The source objects.</param>
    /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
    /// <c>null</c>.</exception>
    public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new HashSet<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }

    /// <summary>
    /// Maps the collection of <typeparamref name="TSource"/> into a hash set of
    /// <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
    /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
    /// <param name="mapper">The mapper.</param>
    /// <param name="source">The source objects.</param>
    /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
    /// <c>null</c>.</exception>
    public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new HashSet<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }

    /// <summary>
    /// Maps the array of <typeparamref name="TSource"/> into a hash set of
    /// <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
    /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
    /// <param name="mapper">The mapper.</param>
    /// <param name="source">The source objects.</param>
    /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
    /// <c>null</c>.</exception>
    public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new HashSet<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }

    /// <summary>
    /// Maps the enumerable of <typeparamref name="TSource"/> into a hash set of
    /// <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects.</typeparam>
    /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
    /// <param name="mapper">The mapper.</param>
    /// <param name="source">The source objects.</param>
    /// <returns>A hash set of <typeparamref name="TDestination"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="mapper"/> or <paramref name="source"/> is
    /// <c>null</c>.</exception>
    public static HashSet<TDestination> MapHashSet<TSource, TDestination>(
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new HashSet<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }

#if NET5_0_OR_GREATER
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source) => ImmutableArray.Create(mapper.MapArray(source));

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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source) => ImmutableList.Create(mapper.MapArray(source));

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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source) =>
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new List<TDestination>(source.Count);
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new List<TDestination>(source.Count);
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new List<TDestination>(source.Length);
        for (var i = 0; i < source.Length; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

#if NET6_0_OR_GREATER
        if (!source.TryGetNonEnumeratedCount(out var count))
        {
            count = source.Count();
        }

        var destination = new List<TDestination>(count);
#else
        var destination = new List<TDestination>(source.Count());
#endif
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        List<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new ObservableCollection<TDestination>();
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        Collection<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new ObservableCollection<TDestination>();
        for (var i = 0; i < source.Count; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        TSource[] source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new ObservableCollection<TDestination>();
        for (var i = 0; i < source.Length; ++i)
        {
            var sourceItem = source[i];
            var destinationItem = mapper.Map(sourceItem);
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
        this IImmutableMapper<TSource, TDestination> mapper,
        IEnumerable<TSource> source)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(source);
#else
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mapper));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
#endif

        var destination = new ObservableCollection<TDestination>();
        foreach (var sourceItem in source)
        {
            var destinationItem = mapper.Map(sourceItem);
            destination.Add(destinationItem);
        }

        return destination;
    }
}
