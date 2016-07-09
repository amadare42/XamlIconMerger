using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class FileElementSource : IElementSource
    {
        private readonly IFileToKeyConverter fileToKeyConverter;
        public readonly string Path;

        public FileElementSource(string path, IFileToKeyConverter fileToKeyConverter)
        {
            this.Path = path;
            this.fileToKeyConverter = fileToKeyConverter;
        }

        public string ElementName
        {
            get { return this.fileToKeyConverter.GetKey(this.Path); }
        }

        public string ElementInfo
        {
            get { return $"file {this.Path}"; }
        }

        public string GetContent()
        {
            return File.ReadAllText(this.Path);
        }
    }
}