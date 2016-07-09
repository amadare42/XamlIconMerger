namespace XamlIconMerger
{
    public interface IElementSource
    {
        string ElementName { get; }

        string ElementInfo { get; }

        string GetContent();
    }
}