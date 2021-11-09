namespace Boxed.Mapping;

/// <summary>
/// Maps an object of type <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.
/// </summary>
/// <typeparam name="TSource">The type of the object to map from.</typeparam>
/// <typeparam name="TDestination">The type of the object to map to.</typeparam>
public interface IImmutableMapper<in TSource, out TDestination>
{
    /// <summary>
    /// Maps the specified source object into the destination object.
    /// </summary>
    /// <param name="source">The source object to map from.</param>
    /// <returns>The destination object to map to.</returns>
    TDestination Map(TSource source);
}
