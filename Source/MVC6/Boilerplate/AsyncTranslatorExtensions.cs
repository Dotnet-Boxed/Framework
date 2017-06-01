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
        /// Translates the collection of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <param name="sourceCount">The number of items in the source collection.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="sourceCollection"/> is
        /// <c>null</c>.</exception>
        public static async Task<TDestination[]> TranslateArray<TSourceCollection, TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSourceCollection sourceCollection,
            TDestination[] destinationCollection,
            int? sourceCount = null)
            where TSourceCollection : IEnumerable<TSource>
            where TDestination : new()
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (sourceCollection == null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            var tasks = new Task[sourceCount ?? sourceCollection.Count()];
            var i = 0;
            foreach (var item in sourceCollection)
            {
                var destination = new TDestination();
                destinationCollection[i] = destination;
                tasks[i] = translator.Translate(item, destination);

                ++i;
            }

            await Task.WhenAll(tasks);

            return destinationCollection;
        }

        /// <summary>
        /// Translates the array of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<TDestination[]> TranslateArray<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Length], source.Length);

        /// <summary>
        /// Translates the list of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<TDestination[]> TranslateArray<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Count], source.Count);

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
        public static Task<TDestination[]> TranslateArray<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Count], source.Count);

        /// <summary>
        /// Translates the enumerable of <typeparamref name="TSource"/> into an array of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<TDestination[]> TranslateArray<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            var sourceCount = source.Count();
            return TranslateArray(translator, source, new TDestination[sourceCount], sourceCount);
        }

        /// <summary>
        /// Translates the collection of <typeparamref name="TSource" /> into a collection of type
        /// <typeparamref name="TDestinationCollection" /> containing objects of type <typeparamref name="TDestination" />.
        /// </summary>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestinationCollection">The type of the destination collection.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <param name="sourceCount">The number of items in the source collection.</param>
        /// <returns>A collection of type <typeparamref name="TDestinationCollection"/> containing objects of type
        /// <typeparamref name="TDestination" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator" /> or <paramref name="sourceCollection" /> is
        /// <c>null</c>.</exception>
        public static async Task<TDestinationCollection> TranslateCollection<TSourceCollection, TSource, TDestinationCollection, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSourceCollection sourceCollection,
            TDestinationCollection destinationCollection,
            int? sourceCount = null)
            where TSourceCollection : IEnumerable<TSource>
            where TDestinationCollection : ICollection<TDestination>
            where TDestination : new()
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (sourceCollection == null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            var tasks = new List<Task>(sourceCount ?? sourceCollection.Count());
            foreach (var item in sourceCollection)
            {
                var destination = new TDestination();
                destinationCollection.Add(destination);
                tasks.Add(translator.Translate(item, destination));
            }

            await Task.WhenAll(tasks);

            return destinationCollection;
        }

        /// <summary>
        /// Translates the list of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<Collection<TDestination>> TranslateCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>(), source.Count);

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
        public static Task<Collection<TDestination>> TranslateCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>(), source.Count);

        /// <summary>
        /// Translates the array of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<Collection<TDestination>> TranslateCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>(), source.Length);

        /// <summary>
        /// Translates the enumerable of <typeparamref name="TSource"/> into a collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<Collection<TDestination>> TranslateCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>());

        /// <summary>
        /// Translates the list of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<List<TDestination>> TranslateList<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Count), source.Count);

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
        public static Task<List<TDestination>> TranslateList<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Count), source.Count);

        /// <summary>
        /// Translates the array of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<List<TDestination>> TranslateList<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Length), source.Length);

        /// <summary>
        /// Translates the enumerable of <typeparamref name="TSource"/> into a list of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>A list of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<List<TDestination>> TranslateList<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new()
        {
            var sourceCount = source.Count();
            return TranslateCollection(translator, source, new List<TDestination>(sourceCount), sourceCount);
        }

        /// <summary>
        /// Translates the list of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<ObservableCollection<TDestination>> TranslateObservableCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>(), source.Count);

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
        public static Task<ObservableCollection<TDestination>> TranslateObservableCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>(), source.Count);

        /// <summary>
        /// Translates the array of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<ObservableCollection<TDestination>> TranslateObservableCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>(), source.Length);

        /// <summary>
        /// Translates the enumerable of <typeparamref name="TSource"/> into an observable collection of
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source objects.</typeparam>
        /// <typeparam name="TDestination">The type of the destination objects.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source objects.</param>
        /// <returns>An observable collection of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="source"/> is
        /// <c>null</c>.</exception>
        public static Task<ObservableCollection<TDestination>> TranslateObservableCollection<TSource, TDestination>(
            this IAsyncTranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>());
    }
}
