using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class StringLazyTextReaderProvider : ILazyTextReaderProvider
    {
        private readonly string text;

        public StringLazyTextReaderProvider(string text)
        {
            this.text = text;
        }

        public TextReader GetTextReader()
        {
            return new StringReader(text);
        }
    }
}