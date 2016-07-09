using System.Diagnostics;

namespace XamlIconMerger
{
    public class DebugOutputTarget : IOutputTarget
    {
        public void AddEntry(IElementSource source, string entry)
        {
            Debug.WriteLine("=========ENTRY:=========");
            Debug.Indent();
            Debug.WriteLine($"{source.ElementName} -> \r\n {entry}");
            Debug.Unindent();
        }
    }
}