namespace Boxed.Mapping;

using System;
using System.Linq.Expressions;

/// <summary>
/// Creates instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type with a parameterless constructor.</typeparam>
public static class Factory<T>
    where T : new()
{
    private static readonly Func<T> CreateInstanceFunc =
        Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

    /// <summary>
    /// Creates an instance of type <typeparamref name="T"/> by calling it's parameterless constructor.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static T CreateInstance() => CreateInstanceFunc();
#pragma warning restore CA1000 // Do not declare static members on generic types
}
