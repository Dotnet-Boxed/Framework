namespace Boxed.AspNetCore.Sitemap
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Generates site-map XML.
    /// </summary>
    public abstract class SitemapGenerator
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        /// <summary>
        /// The maximum number of site-maps a site-map index file can contain.
        /// </summary>
        private const int MaximumSitemapCount = 50000;

        /// <summary>
        /// The maximum number of site-map nodes allowed in a site-map file. The absolute maximum allowed is 50,000
        /// according to the specification. See http://www.sitemaps.org/protocol.html but the file size must also be
        /// less than 10MB. After some experimentation, a maximum of 25,000 nodes keeps the file size below 10MB.
        /// </summary>
        private const int MaximumSitemapNodeCount = 25000;

        /// <summary>
        /// The maximum size of a site-map file in bytes (10MB).
        /// </summary>
        private const int MaximumSitemapSizeInBytes = 10485760;

        /// <summary>
        /// Gets the collection of XML site-map documents for the current site. If there are less than 25,000 site-map
        /// nodes, only one site-map document will exist in the collection, otherwise a site-map index document will be
        /// the first entry in the collection and all other entries will be site-map XML documents.
        /// </summary>
        /// <param name="sitemapNodes">The site-map nodes for the current site.</param>
        /// <returns>A collection of XML site-map documents.</returns>
        protected virtual List<string> GetSitemapDocuments(IReadOnlyCollection<SitemapNode> sitemapNodes)
        {
            var sitemapCount = (int)Math.Ceiling(sitemapNodes.Count / (double)MaximumSitemapNodeCount);
            this.CheckSitemapCount(sitemapCount);
            var sitemaps = Enumerable
                .Range(0, sitemapCount)
                .Select(x =>
                    {
                        return new KeyValuePair<int, IEnumerable<SitemapNode>>(
                            x + 1,
                            sitemapNodes.Skip(x * MaximumSitemapNodeCount).Take(MaximumSitemapNodeCount));
                    });

            var sitemapDocuments = new List<string>(sitemapCount);

            if (sitemapCount > 1)
            {
                var xml = this.GetSitemapIndexDocument(sitemaps);
                sitemapDocuments.Add(xml);
            }

            foreach (var sitemap in sitemaps)
            {
                var xml = this.GetSitemapDocument(sitemap.Value);
                sitemapDocuments.Add(xml);
            }

            return sitemapDocuments;
        }

        /// <summary>
        /// Gets the URL to the site-map with the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The site-map URL.</returns>
        protected abstract string GetSitemapUrl(int index);

        /// <summary>
        /// Logs warnings when a site-map exceeds the maximum size of 10MB or if the site-map index file exceeds the
        /// maximum number of allowed site-maps. No exceptions are thrown in this case as the site-map file is still
        /// generated correctly and search engines may still read it.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        protected virtual void LogWarning(Exception exception)
        {
        }

        /// <summary>
        /// Gets the site-map index XML document, containing links to all the site-map XML documents.
        /// </summary>
        /// <param name="sitemaps">The collection of site-maps containing their index and nodes.</param>
        /// <returns>The site-map index XML document, containing links to all the site-map XML documents.</returns>
        private string GetSitemapIndexDocument(IEnumerable<KeyValuePair<int, IEnumerable<SitemapNode>>> sitemaps)
        {
            var xmlns = XNamespace.Get(SitemapsNamespace);
            var root = new XElement(xmlns + "sitemapindex");

            foreach (var sitemap in sitemaps)
            {
                // Get the latest LastModified DateTime from the sitemap nodes or null if there is none.
                var lastModified = sitemap.Value
                    .Select(x => x.LastModified)
                    .Where(x => x.HasValue)
                    .DefaultIfEmpty()
                    .Max();

                var lastModifiedElement = lastModified.HasValue ?
                    new XElement(
                        xmlns + "lastmod",
                        lastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")) :
                    null;
                var sitemapElement = new XElement(
                    xmlns + "sitemap",
                    new XElement(xmlns + "loc", this.GetSitemapUrl(sitemap.Key)),
                    lastModifiedElement);

                root.Add(sitemapElement);
            }

            var document = new XDocument(root);
            var xml = document.ToString(Encoding.UTF8);
            this.CheckDocumentSize(xml);
            return xml;
        }

        /// <summary>
        /// Gets the site-map XML document for the specified set of nodes.
        /// </summary>
        /// <param name="sitemapNodes">The site-map nodes.</param>
        /// <returns>The site-map XML document for the specified set of nodes.</returns>
        private string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            var xmlns = XNamespace.Get(SitemapsNamespace);
            var root = new XElement(xmlns + "urlset");

            foreach (var sitemapNode in sitemapNodes)
            {
                var lastModifiedElement = sitemapNode.LastModified.HasValue ?
                    new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")) :
                    null;
                var frequencyElement = sitemapNode.Frequency.HasValue ?
                    new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()) :
                    null;
                var priorityElement = sitemapNode.Priority.HasValue ?
                    new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)) :
                    null;

                var urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    lastModifiedElement,
                    frequencyElement,
                    priorityElement);
                root.Add(urlElement);
            }

            var document = new XDocument(root);
            var xml = document.ToString(Encoding.UTF8);
            this.CheckDocumentSize(xml);
            return xml;
        }

        /// <summary>
        /// Checks the size of the XML site-map document. If it is over 10MB, logs an error.
        /// </summary>
        /// <param name="sitemapXml">The site-map XML document.</param>
        private void CheckDocumentSize(string sitemapXml)
        {
            if (sitemapXml.Length >= MaximumSitemapSizeInBytes)
            {
                this.LogWarning(new SitemapException(
                    $"Sitemap exceeds the maximum size of 10MB. This is because you have unusually long URL's. Consider reducing the MaximumSitemapNodeCount. Size:<{sitemapXml.Length}>."));
            }
        }

        /// <summary>
        /// Checks the count of the number of site-maps. If it is over 50,000, logs an error.
        /// </summary>
        /// <param name="sitemapCount">The site-maps count.</param>
        private void CheckSitemapCount(int sitemapCount)
        {
            if (sitemapCount > MaximumSitemapCount)
            {
                this.LogWarning(new SitemapException(
                    $"Sitemap index file exceeds the maximum number of allowed sitemaps of 50,000. Count:<{sitemapCount}>"));
            }
        }
    }
}