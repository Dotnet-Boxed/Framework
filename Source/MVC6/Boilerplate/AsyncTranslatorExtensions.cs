namespace Boilerplate
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="IAsyncTranslator{TSource, TDestination}"/> extension methods.
    /// </summary>
    public static class AsyncTranslatorExtensions
    {
        /// <summary>
        /// Translates the specified source object to a new object with a type of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDestination">The type of the destination object.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source object.</param>
        /// <returns>The translated object of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination> Translate<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSource source)
            where TDestination : new()
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = new TDestination();
            await translator.Translate(source, destination);
            return destination;
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource" /> into a collection of type
        /// <typeparamref name="TCollection" /> containing objects of type <typeparamref name="TDestination" />.
        /// </summary>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of type <typeparamref name="TCollection"/> containing objects of type
        /// <typeparamref name="TDestination" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static async Task<TCollection> TranslateCollection<TCollection, TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TCollection : ICollection<TDestination>, new()
            where TDestination : new()
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var collection = new TCollection();
            var tasks = new List<Task>(source.Count());

            foreach (var item in source)
            {
                var destination = new TDestination();
                collection.Add(destination);

                var task = translator.Translate(item, destination);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            return collection;
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<Collection<TDestination>> TranslateCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            return await TranslateCollection<Collection<TDestination>, TSource, TDestination>(translator, source);
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<List<TDestination>> TranslateList<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            return await TranslateCollection<List<TDestination>, TSource, TDestination>(translator, source);
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<ObservableCollection<TDestination>> TranslateObservableCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            return await TranslateCollection<ObservableCollection<TDestination>, TSource, TDestination>(translator, source);
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> TranslateArray<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var array = new TDestination[source.Count()];
            var tasks = new Task[source.Count()];

            int i = 0;
            foreach (var item in source)
            {
                var destination = new TDestination();
                array[i] = destination;

                var task = translator.Translate(item, destination);
                tasks[i] = task;

                ++i;
            }

            await Task.WhenAll(tasks);

            return array;
        }
    }
}
