using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class FileNameToKeyConverter : IFileToKeyConverter
    {
        private readonly string elementName;

        public FileNameToKeyConverter(string elementName)
        {
            this.elementName = elementName;
        }

        public string GetKey(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var iconPresentInName = name.Contains("Icon");
            name = char.ToUpperInvariant(name[0]) + name.Substring(1);
            if (!iconPresentInName)
            {
                name += "Icon";
            }
            name += elementName;

            return name;
        }
    }
}