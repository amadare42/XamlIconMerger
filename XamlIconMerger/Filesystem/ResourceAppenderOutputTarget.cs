using System;

namespace XamlIconMerger.Filesystem
{
    public class ResourceAppenderOutputTarget : IOutputTarget
    {
        private readonly XamlAppender appender;
        private readonly ElementDuplicationPolicy duplicationPolicy;
        private readonly string filePath;
        private readonly string outFilePath;
        private readonly ILazyTextWriterProvider textWriterProvider;

        public ResourceAppenderOutputTarget(
            ILazyTextReaderProvider textReaderProvider,
            IAppendNodeExtractor appendNodeExtractor,
            ILazyTextWriterProvider textWriterProvider,
            ElementDuplicationPolicy duplicationPolicy)
        {
            this.textWriterProvider = textWriterProvider;
            this.duplicationPolicy = duplicationPolicy;

            appender = new XamlAppender(textReaderProvider, appendNodeExtractor, new[] { "x:Key", "Key" });
        }

        public void AddEntry(IElementSource source, string entry)
        {
            switch (duplicationPolicy)
            {
                case ElementDuplicationPolicy.Ignore:
                    appender.AppendToTarget(entry);
                    break;

                case ElementDuplicationPolicy.Update:
                    appender.ReplaceWithEntry(source.ElementName, entry);
                    break;

                default:
                    throw new InvalidOperationException("Unknown Duplication policy.");
            }
            using (var writer = textWriterProvider.GetTextWriter())
            {
                appender.SaveDocument(writer);
            }
        }
    }
}