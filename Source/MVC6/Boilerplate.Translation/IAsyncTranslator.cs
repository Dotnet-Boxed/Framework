namespace Boilerplate.Translation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Translates an object of type <typeparamref name="TSource"/> to <typeparamref name="TDestination"/> asynchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the object to translate from.</typeparam>
    /// <typeparam name="TDestination">The type of the object to translate to.</typeparam>
    public interface IAsyncTranslator<in TSource, in TDestination>
    {
        /// <summary>
        /// Translates the specified source object into the destination object.
        /// </summary>
        /// <param name="source">The source object to copy from.</param>
        /// <param name="destination">The destination object to copy to.</param>
        /// <returns>A task representing the operation.</returns>
        Task Translate(TSource source, TDestination destination);
    }
}
