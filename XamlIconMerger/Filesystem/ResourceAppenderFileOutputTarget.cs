using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace XamlIconMerger.Filesystem
{
    public class ResourceAppenderFileOutputTarget : IOutputTarget
    {
        private readonly string filePath;
        private readonly string regionName;
        private readonly ElementDuplicationPolicy duplicationPolicy;
        private readonly string outFilePath;

        private XmlDocument doc;
        private Dictionary<string, XmlNode> keyNodeDict;
        private XmlNode appendNode;
        private bool inited;

        public ResourceAppenderFileOutputTarget(string filePath, string regionName, string outFilePath = null, ElementDuplicationPolicy duplicationPolicy = ElementDuplicationPolicy.Update)
        {
            this.filePath = filePath;
            this.regionName = regionName;
            this.outFilePath = outFilePath ?? filePath;
            this.duplicationPolicy = duplicationPolicy;
        }

        private void Init()
        {
            doc = LoadDocument(this.filePath);
            MakeReserveCopy(this.filePath);

            keyNodeDict = GetKeyNodeDictionary(doc);
            appendNode = GetAppendNode(doc, regionName);
            this.inited = true;
        }

        private static XmlDocument LoadDocument(string path)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                IgnoreComments = false,
                IgnoreWhitespace = false
            };
            var reader = XmlReader.Create(path, readerSettings);
            doc.Load(reader);
            reader.Close();

            return doc;
        }

        private static void MakeReserveCopy(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string directory = Path.GetDirectoryName(filePath) + Path.PathSeparator;
            string extension = Path.GetExtension(filePath);
            var copyPath = $"{directory}{fileName}_orig{extension}";
            File.Copy(filePath, copyPath, true);
        }

        public static XmlNode GetAppendNode(XmlDocument doc, string regionName)
        {
            var nodeValue = $"#region {regionName}";
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment && node.Value == nodeValue)
                    return node;
            }
            return null;
        }

        public static Dictionary<string, XmlNode> GetKeyNodeDictionary(XmlDocument doc)
        {
            var dict = new Dictionary<string, XmlNode>();

            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                if (node.Attributes == null)
                    continue;
                var nameAttr = node.Attributes.Cast<XmlAttribute>()
                    .FirstOrDefault(attr => attr.Name == "x:Key");
                if (nameAttr != null)
                {
                    dict.Add(nameAttr.Value, node);
                }
            }

            return dict;
        }

        private void ReplaceWithEntry(string key, string text)
        {
            var prevNode = this.keyNodeDict[key];
            prevNode.ParentNode.RemoveChild(prevNode);

            AppendToTarget(text);
        }

        private void AppendToTarget(string text, bool addWhiteSpace = true)
        {
            if (this.appendNode.ParentNode == null)
            {
                throw new InvalidOperationException("Append node doesn't have a parent.");
            }
            var appendToNode = this.appendNode.ParentNode;

            var refNode = this.appendNode;
            if (addWhiteSpace)
            {
                refNode = AddWhitespace(appendToNode, refNode);
            }
            var textFrag = doc.CreateDocumentFragment();
            textFrag.InnerXml = text;
            appendToNode.InsertAfter(textFrag, refNode);
        }

        private static XmlNode AddWhitespace(XmlNode appendToNode, XmlNode refNode)
        {
            if (appendToNode.OwnerDocument == null)
            {
                throw new InvalidOperationException("Append node doesn't have owner.");
            }
            var whitespace = appendToNode.OwnerDocument.CreateWhitespace("\n    ");
            appendToNode.InsertAfter(whitespace, refNode);
            return whitespace;
        }

        public void AddEntry(IElementSource source, string entry)
        {
            if (!this.inited)
                this.Init();

            switch (duplicationPolicy)
            {
                case ElementDuplicationPolicy.Ignore:
                    AppendToTarget(entry);
                    break;

                case ElementDuplicationPolicy.Update:
                    ReplaceWithEntry(source.ElementName, entry);
                    break;

                default:
                    throw new InvalidOperationException("Unknown Duplication policy.");
            }

            this.doc.Save(this.outFilePath);
        }
    }

    public enum ElementDuplicationPolicy
    {
        Update,
        Ignore
    }
}