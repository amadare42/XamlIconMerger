using System.Xml;

namespace XamlIconMerger.Mutators.Xml
{
    public class TagExtractor : IXmlMutator
    {
        private readonly string elementName;

        public TagExtractor(string elementName)
        {
            this.elementName = elementName;
        }

        public XmlNode Mutate(XmlNode node, IElementSource source = null)
        {
            return node.SelectSingleNode($"//{this.elementName}");
        }
    }
}