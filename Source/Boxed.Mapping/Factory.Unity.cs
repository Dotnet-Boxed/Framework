namespace Boxed.Mapping;

using System.Runtime.CompilerServices;

/// <summary>
/// Creates instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type with a parameterless constructor.</typeparam>
public static class Factory<T>
    where T : new()
{
    /// <summary>
    /// Creates an instance of type <typeparamref name="T"/> by calling it's parameterless constructor.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static T CreateInstance() => new();
#pragma warning restore CA1000 // Do not declare static members on generic types
}
