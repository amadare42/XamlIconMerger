using System.IO;

namespace XamlIconMerger.Filesystem
{
    public interface ILazyTextReaderProvider
    {
        TextReader GetTextReader();
    }
}