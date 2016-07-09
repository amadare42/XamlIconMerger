using System.Xml;

namespace XamlIconMerger.Mutators.Xml
{
    public class KeyAddingMutator : IXmlMutator
    {
        private readonly string elementName;
        private readonly string keyName;

        public KeyAddingMutator(string keyName, string elementName)
        {
            this.keyName = keyName;
            this.elementName = elementName;
        }

        public KeyAddingMutator(string keyName = "Key")
        {
            this.keyName = keyName;
            elementName = null;
        }

        public XmlNode Mutate(XmlNode node, IElementSource source)
        {
            var selectedNode = this.elementName == null
                ? node
                : node.SelectSingleNode($"//{this.elementName}");
            var doc = node.OwnerDocument;
            var attr = doc.CreateAttribute(keyName);
            attr.Value = source.ElementName;
            selectedNode.Attributes.Append(attr);
            return selectedNode;
        }
    }
}