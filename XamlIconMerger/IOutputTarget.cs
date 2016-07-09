namespace XamlIconMerger
{
    public interface IOutputTarget
    {
        void AddEntry(IElementSource source, string entry);
    }
}