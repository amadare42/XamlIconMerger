using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace XamlIconMerger.Filesystem
{
    public class XamlAppender
    {
        private readonly IAppendNodeExtractor appendNodeExtractor;
        private readonly IList<string> keyAttributes;
        private readonly ILazyTextReaderProvider textReaderProvider;
        private XmlNode appendNode;
        private XmlDocument Document;
        public bool Inited;

        private Dictionary<string, XmlNode> keyNodeDict;

        public XamlAppender(ILazyTextReaderProvider textReaderProvider, IAppendNodeExtractor appendNodeExtractor,
            IList<string> keyAttributes)
        {
            this.textReaderProvider = textReaderProvider;
            this.appendNodeExtractor = appendNodeExtractor;
            this.keyAttributes = keyAttributes;
        }

        private static XmlDocument LoadDocument(TextReader textReader)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            var readerSettings = new XmlReaderSettings
            {
                IgnoreComments = false,
                IgnoreWhitespace = false
            };

            var reader = XmlReader.Create(textReader, readerSettings);
            doc.Load(reader);
            reader.Close();

            doc.DocumentElement.SetAttribute("xmlns:x", @"http://schemas.microsoft.com/winfx/2006/xaml\");
            return doc;
        }

        public void ReplaceWithEntry(string key, string text)
        {
            if (!Inited)
                Init();
            XmlNode oldNode;
            if (keyNodeDict.TryGetValue(key, out oldNode))
            {
                oldNode.ParentNode.RemoveChild(oldNode);
            }

            AppendToTarget(text);
        }

        public void AppendToTarget(string text, bool addWhiteSpace = true)
        {
            if (!Inited)
                Init();
            if (appendNode.ParentNode == null)
            {
                throw new InvalidOperationException("Append node doesn't have a parent.");
            }
            var appendToNode = appendNode.ParentNode;

            var refNode = appendNode;
            if (addWhiteSpace)
            {
                refNode = AddWhitespace(appendToNode, appendNode);
            }

            var nodeDoc = new XmlDocument(Document.NameTable);
            nodeDoc.LoadXml(text);

            var textFrag = Document.CreateDocumentFragment();
            textFrag.InnerXml = text;
            var newNode = appendToNode.InsertAfter(textFrag, refNode);
            AddNewKeyToDictionary(newNode, keyNodeDict, keyAttributes);
        }

        public void SaveDocument(TextWriter writer)
        {
            Document.Save(writer);
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

        private void Init()
        {
            using (var textReader = textReaderProvider.GetTextReader())
            {
                Document = LoadDocument(textReader);
            }

            keyNodeDict = GetKeyNodeDictionary(Document, keyAttributes);
            appendNode = appendNodeExtractor.GetAppendNode(Document);

            Inited = true;
        }

        private static void AddNewKeyToDictionary(XmlNode node, Dictionary<string, XmlNode> dict, IList<string> keys)
        {
            if (node.Attributes == null)
                return;
            var nameAttr = node.Attributes.Cast<XmlAttribute>()
                .FirstOrDefault(attr => keys.Contains(attr.Name));
            if (nameAttr != null)
            {
                dict.Add(nameAttr.Value, node);
            }
        }

        public static Dictionary<string, XmlNode> GetKeyNodeDictionary(XmlDocument doc, IList<string> keys)
        {
            var dict = new Dictionary<string, XmlNode>();

            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                AddNewKeyToDictionary(node, dict, keys);
            }

            return dict;
        }
    }
}