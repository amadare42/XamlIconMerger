using System.Xml;

namespace XamlIconMerger.Mutators
{
    public class XmlMutatorChain : MutatorChain<XmlNode>, IXmlMutator
    {
        public XmlMutatorChain(params IMutator<XmlNode>[] mutators) : base(mutators)
        {
        }
    }
}