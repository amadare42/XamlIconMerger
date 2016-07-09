using System.Collections.Generic;

namespace XamlIconMerger.Filesystem
{
    public interface IFileFetchOptions
    {
        IEnumerable<string> FileList { get; }
        IEnumerable<string> FileMasksList { get; }
        string OutFile { get; }
        string RootDirectory { get; }
        bool Recursive { get; }
    }
}