using System;
using System.IO;

namespace XamlIconMerger.Filesystem
{
    public class WriteToFileOutputTarget : IOutputTarget
    {
        private readonly string outPath;

        public WriteToFileOutputTarget(string outPath)
        {
            this.outPath = outPath;
            File.WriteAllText(outPath, string.Empty);
        }

        public void AddEntry(IElementSource source, string entry)
        {
            File.AppendAllText(this.outPath, entry + Environment.NewLine);
        }
    }
}