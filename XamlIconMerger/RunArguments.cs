using CommandLine;
using System.Collections.Generic;
using System.IO;
using XamlIconMerger.Filesystem;

namespace XamlIconMerger
{
    public class RunArguments : IFileFetchOptions
    {
        public RunArguments()
        {
            RootDirectory = Directory.GetCurrentDirectory();
        }

        [Option('f', "files", HelpText = "Files to clean")]
        public IEnumerable<string> FileList { get; set; }

        [Option('m', "filesMasks", HelpText = "Masks for files to search in search directory")]
        public IEnumerable<string> FileMasksList { get; set; }

        [Option('d', "directory", HelpText = "Root directory")]
        public string RootDirectory { get; set; }

        [Option('o', "out", HelpText = "Out file", Required = true)]
        public string OutFile { get; set; }

        [Option('r', "recursive", HelpText = "Whether or not search files in inner folders")]
        public bool Recursive { get; set; }
    }
}