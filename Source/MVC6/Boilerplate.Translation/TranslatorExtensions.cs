namespace Boilerplate.Translation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// <see cref="ITranslator{TSource, TDestination}"/> extension methods.
    /// </summary>
    public static class TranslatorExtensions
    {
        /// <summary>
        /// Translates the specified source object to a new object with a type of <typeparamref name="TDestination"/>.
        /// </summary>dotnet test
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDestination">The type of the destination object.</typeparam>
        /// <param name="translator">The translator.</param>
        /// <param name="source">The source object.</param>
        /// <returns>The translated object of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator" /> or <paramref name="source" /> is
        /// <c>null</c>.</exception>
        public static TDestination Translate<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
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
            translator.Translate(source, destination);
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
        /// <returns>An array of <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator"/> or <paramref name="sourceCollection"/> is
        /// <c>null</c>.</exception>
        public static TDestination[] TranslateArray<TSourceCollection, TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSourceCollection sourceCollection,
            TDestination[] destinationCollection)
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

            var i = 0;
            foreach (var item in sourceCollection)
            {
                var destination = new TDestination();
                translator.Translate(item, destination);
                destinationCollection[i] = destination;

                ++i;
            }

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
        public static TDestination[] TranslateArray<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Length]);

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
        public static TDestination[] TranslateArray<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Count]);

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
        public static TDestination[] TranslateArray<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Count]);

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
        public static TDestination[] TranslateArray<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            TranslateArray(translator, source, new TDestination[source.Count()]);

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
        /// <returns>A collection of type <typeparamref name="TDestinationCollection"/> containing objects of type
        /// <typeparamref name="TDestination" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="translator" /> or <paramref name="sourceCollection" /> is
        /// <c>null</c>.</exception>
        public static TDestinationCollection TranslateCollection<TSourceCollection, TSource, TDestinationCollection, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSourceCollection sourceCollection,
            TDestinationCollection destinationCollection)
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

            foreach (var item in sourceCollection)
            {
                var destination = new TDestination();
                translator.Translate(item, destination);
                destinationCollection.Add(destination);
            }

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
        public static Collection<TDestination> TranslateCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>());

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
        public static Collection<TDestination> TranslateCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>());

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
        public static Collection<TDestination> TranslateCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new Collection<TDestination>());

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
        public static Collection<TDestination> TranslateCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
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
        public static List<TDestination> TranslateList<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Count));

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
        public static List<TDestination> TranslateList<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Count));

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
        public static List<TDestination> TranslateList<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Length));

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
        public static List<TDestination> TranslateList<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new List<TDestination>(source.Count()));

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
        public static ObservableCollection<TDestination> TranslateObservableCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            List<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>());

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
        public static ObservableCollection<TDestination> TranslateObservableCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            Collection<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>());

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
        public static ObservableCollection<TDestination> TranslateObservableCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            TSource[] source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>());

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
        public static ObservableCollection<TDestination> TranslateObservableCollection<TSource, TDestination>(
            this ITranslator<TSource, TDestination> translator,
            IEnumerable<TSource> source)
            where TDestination : new() =>
            TranslateCollection(translator, source, new ObservableCollection<TDestination>());
    }
}
