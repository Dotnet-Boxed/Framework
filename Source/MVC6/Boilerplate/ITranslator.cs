namespace Framework
{
    /// <summary>
    /// Translates an object of type <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the object to translate from.</typeparam>
    /// <typeparam name="TDestination">The type of the object to translate to.</typeparam>
    public interface ITranslator<in TSource, in TDestination>
    {
        /// <summary>
        /// Translates the specified source object into the destination object.
        /// </summary>
        /// <param name="source">The source object to copy from.</param>
        /// <param name="destination">The destination object to copy to.</param>
        void Translate(TSource source, TDestination destination);
    }
}
