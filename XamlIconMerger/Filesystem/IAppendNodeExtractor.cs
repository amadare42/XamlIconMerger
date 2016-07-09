using System.Xml;

namespace XamlIconMerger.Filesystem
{
    public interface IAppendNodeExtractor
    {
        XmlNode GetAppendNode(XmlDocument doc);
    }
}