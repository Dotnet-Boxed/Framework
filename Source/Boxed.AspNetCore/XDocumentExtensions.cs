namespace Boxed
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// <see cref="XDocument"/> extension methods.
    /// </summary>
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Returns a <see cref="string" /> that represents the XML document in the specified encoding.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// A <see cref="string" /> that represents the XML document.
        /// </returns>
        public static string ToString(this XDocument document, Encoding encoding)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var stringBuilder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriterWithEncoding(stringBuilder, encoding))
            {
                document.Save(stringWriter);
            }

            return stringBuilder.ToString();
        }
    }
}