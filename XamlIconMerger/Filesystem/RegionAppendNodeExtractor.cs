using System.IO;
using System.Xml;

namespace XamlIconMerger.Filesystem
{
    public class RegionAppendNodeExtractor : IAppendNodeExtractor
    {
        private readonly string regionName;

        public RegionAppendNodeExtractor(string regionName)
        {
            this.regionName = regionName;
        }

        public XmlNode GetAppendNode(XmlDocument doc)
        {
            var nodeValue = $"#region {regionName}";
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment && node.Value == nodeValue)
                    return node;
            }
            throw new InvalidDataException($"Cannot find {regionName} region.");
        }
    }
}