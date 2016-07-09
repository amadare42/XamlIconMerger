using System.IO;

namespace XamlIconMerger.Filesystem
{
    public interface ILazyTextWriterProvider
    {
        TextWriter GetTextWriter();
    }
}