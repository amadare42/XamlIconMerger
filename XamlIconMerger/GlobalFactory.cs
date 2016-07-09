using XamlIconMerger.Filesystem;
using XamlIconMerger.Messages;
using XamlIconMerger.Mutators;
using XamlIconMerger.Mutators.Text;
using XamlIconMerger.Mutators.Xml;

namespace XamlIconMerger
{
    public class GlobalFactory
    {
        private readonly ILoggingService loggingService;

        public GlobalFactory(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public TextMerger CreateDefaultTextMerger(IFileFetchOptions options)
        {
            IOutputTarget outputTarget = new WriteToFileOutputTarget(options.OutFile);

            var xmlMutator = new XmlMutatorChain(
                new TagExtractor("Canvas"),
                new KeyAddingMutator());

            var preTextMutator = new TextMutatorChain(
                new RegexpCleanerMutator());

            var postTextMutator = new TextMutatorChain(
                new AddPrefixToAttributesTextMutator("Key", "x"));

            return new TextMerger(
                preTextMutator,
                xmlMutator,
                postTextMutator,
                outputTarget,
                this.loggingService);
        }

        public FileFetcher CreateDefaultFileFetcher()
        {
            var fileToKeyConverter = new FileNameToKeyConverter("Canvas");

            return new FileFetcher(this.loggingService, fileToKeyConverter);
        }
    }
}