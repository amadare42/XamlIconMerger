using System;
using System.IO;
using System.Text;
using System.Xml;

namespace XamlIconMerger.XmlHelpers
{
    /// <summary>
    ///     Supports formatting for XML in a format that is easily human-readable.
    /// </summary>
    public static class PrettyXmlFormatter
    {
        private static XmlWriterSettings DefaultSettings => new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = Environment.NewLine,
            NewLineHandling = NewLineHandling.Replace,
            //NewLineOnAttributes = true,
            OmitXmlDeclaration = true,
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            NewLineOnAttributes = true
        };

        /// <summary>
        ///     Generates formatted UTF-8 XML for the content in the <paramref name="doc" />
        /// </summary>
        /// <param name="doc">XmlDocument for which content will be returned as a formatted string</param>
        /// <returns>Formatted (indented) XML string</returns>
        public static string GetPrettyXml(XmlDocument doc)
        {
            // Configure how XML is to be formatted
            var settings = DefaultSettings;

            // Use wrapper class that supports UTF-8 encoding
            var sw = new StringWriterWithEncoding(Encoding.UTF8);

            // Output formatted XML to StringWriter
            using (var writer = XmlWriter.Create(sw, settings))
            {
                doc.Save(writer);
            }

            // Get formatted text from writer
            return sw.ToString();
        }

        /// <summary>
        ///     Generates formatted UTF-8 XML for the content in the <paramref name="doc" />
        /// </summary>
        /// <param name="doc">XmlDocument for which content will be returned as a formatted string</param>
        /// <returns>Formatted (indented) XML string</returns>
        public static string GetPrettyXml(XmlNode doc)
        {
            // Configure how XML is to be formatted
            var settings = DefaultSettings;

            // Use wrapper class that supports UTF-8 encoding
            var sw = new StringWriterWithEncoding(Encoding.UTF8);

            // Output formatted XML to StringWriter
            using (var writer = XmlWriter.Create(sw, settings))
            {
                doc.WriteTo(writer);
            }

            // Get formatted text from writer
            return sw.ToString();
        }

        /// <summary>
        ///     Wrapper class around <see cref="StringWriter" /> that supports encoding.
        ///     Attribution: http://stackoverflow.com/a/427737/3063884
        /// </summary>
        private sealed class StringWriterWithEncoding : StringWriter
        {
            /// <summary>
            ///     Creates a new <see cref="PrettyXmlFormatter" /> with the specified encoding
            /// </summary>
            /// <param name="encoding"></param>
            public StringWriterWithEncoding(Encoding encoding)
            {
                this.Encoding = encoding;
            }

            /// <summary>
            ///     Encoding to use when dealing with text
            /// </summary>
            public override Encoding Encoding { get; }
        }
    }
}