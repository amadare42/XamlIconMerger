using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class FileTextWriterLazyProvider : ILazyTextWriterProvider
    {
        private readonly string filePath;

        public FileTextWriterLazyProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public TextWriter GetTextWriter()
        {
            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            return new StreamWriter(fileStream);
        }
    }
}