using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XamlIconMerger.Messages;

namespace XamlIconMerger.Filesystem
{
    public class FileFetcher
    {
        private readonly IFileToKeyConverter fileToKeyConverter;
        private readonly ILoggingService loggingService;

        public FileFetcher(ILoggingService loggingService, IFileToKeyConverter fileToKeyConverter)
        {
            this.loggingService = loggingService;
            this.fileToKeyConverter = fileToKeyConverter;
        }

        public IEnumerable<FileElementSource> GetFilesList(IFileFetchOptions args)
        {
            var paths = this.GetPaths(args);
            return paths.Select(PathToDataSource);
        }

        private FileElementSource PathToDataSource(string path)
        {
            return new FileElementSource(path, this.fileToKeyConverter);
        }

        private IEnumerable<string> GetPaths(IFileFetchOptions args)
        {
            if (!args.FileList.Any() && !args.FileMasksList.Any())
            {
                return this.GetAllFilesInRoot(args);
            }
            return this.GetListedFiles(args);
        }

        private IEnumerable<string> GetAllFilesInRoot(IFileFetchOptions args)
        {
            var searchOption = args.Recursive
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = Directory.EnumerateFiles(args.RootDirectory, "*.xaml", searchOption).ToList();
            var outFileIndex = files.FindIndex(path => this.ComparePaths(path, args.OutFile));
            if (outFileIndex >= 0)
            {
                this.loggingService.LogInfo("Found out file, excluding it from selection.");
                files.RemoveAt(outFileIndex);
            }
            return files;
        }

        private IEnumerable<string> GetListedFiles(IFileFetchOptions args)
        {
            var searchOption = args.Recursive
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = new List<string>();
            if (args.FileList != null)
            {
                var errorFile = args.FileList.FirstOrDefault(file => !File.Exists(file));
                if (errorFile != null)
                {
                    throw new Exception($"Cannot find file '{Path.GetFullPath(errorFile)}'.");
                }
                files.AddRange(args.FileList);
            }
            if (args.FileMasksList != null)
            {
                foreach (var mask in args.FileMasksList)
                {
                    files.AddRange(Directory.EnumerateFiles(args.RootDirectory, mask, searchOption));
                }
            }
            return files;
        }

        private bool ComparePaths(string path1, string path2)
        {
            return Path.GetFullPath(path1) == Path.GetFullPath(path2);
        }
    }
}