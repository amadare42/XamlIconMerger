using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class FileLazyTextReaderProvider : ILazyTextReaderProvider
    {
        private readonly string filePath;
        private readonly bool makeCopy;

        public FileLazyTextReaderProvider(string filePath, bool makeCopy = true)
        {
            this.filePath = filePath;
            this.makeCopy = makeCopy;
        }

        public TextReader GetTextReader()
        {
            if (makeCopy)
            {
                MakeReserveCopy(filePath);
            }
            var fileStream = new FileStream(filePath, FileMode.Open);
            return new StreamReader(fileStream);
        }

        private static void MakeReserveCopy(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var directory = Path.GetDirectoryName(filePath) + Path.PathSeparator;
            var extension = Path.GetExtension(filePath);
            var copyPath = $"{directory}{fileName}_orig{extension}";
            File.Copy(filePath, copyPath, true);
        }
    }
}