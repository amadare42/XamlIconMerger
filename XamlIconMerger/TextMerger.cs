using System;
using System.Collections.Generic;
using System.Xml;
using XamlIconMerger.Infrastructure;
using XamlIconMerger.Messages;
using XamlIconMerger.Mutators;
using XamlIconMerger.XmlHelpers;

namespace XamlIconMerger
{
    public class TextMerger
    {
        private readonly ILoggingService loggingService;
        private readonly IOutputTarget outputTarget;
        private readonly ITextMutator postTextMutator;
        private readonly ITextMutator preTextMutator;
        private readonly IXmlMutator xmlMutator;

        public TextMerger(
            ITextMutator preTextMutator,
            IXmlMutator xmlMutator,
            ITextMutator postTextMutator,
            IOutputTarget outputTarget,
            ILoggingService loggingService)
        {
            this.preTextMutator = preTextMutator;
            this.xmlMutator = xmlMutator;
            this.postTextMutator = postTextMutator;
            this.outputTarget = outputTarget;
            this.loggingService = loggingService;
        }

        public void Merge(IEnumerable<IElementSource> sources,
            ErrorHandlingPolicy errorPolicy = ErrorHandlingPolicy.ThrowOnFirstError)
        {
            int mutated = 0, errors = 0;
            foreach (var source in sources)
            {
                try
                {
                    using (var block = loggingService.NewBlock(source.ElementInfo))
                    {
                        block.Log("[fetching]");
                        var text = source.GetContent();

                        if (this.preTextMutator != null)
                        {
                            block.Log("[pre-xml]");
                            text = this.preTextMutator.Mutate(text, source);
                        }

                        if (this.xmlMutator != null)
                        {
                            block.Log("[parse XML]");
                            var xmlDoc = ParseXmlDocument(text);

                            block.Log("[modifying XML]");
                            xmlDoc = this.xmlMutator.Mutate(xmlDoc, source);
                            text = PrettyXmlFormatter.GetPrettyXml(xmlDoc);
                        }

                        if (this.postTextMutator != null)
                        {
                            block.Log("[post-xml]");
                            text = this.postTextMutator.Mutate(text, source);
                        }

                        this.outputTarget.AddEntry(source, text);
                        block.Log("[saved]");
                    }
                    mutated++;
                }
                catch (Exception ex)
                {
                    switch (errorPolicy)
                    {
                        case ErrorHandlingPolicy.ThrowOnFirstError:
                            throw;
                        case ErrorHandlingPolicy.IgnoreErrors:
                            errors++;
                            continue;
                        case ErrorHandlingPolicy.LogOnError:
                            this.loggingService.LogException(ex);
                            errors++;
                            break;
                    }
                }
            }

            this.loggingService.LogInfo($"Done merging! Added {mutated} entries. {errors} completed with errors.");
        }

        private XmlNode ParseXmlDocument(string xmlText)
        {
            var doc = new XmlDocument {InnerXml = xmlText};
            return doc;
        }
    }
}