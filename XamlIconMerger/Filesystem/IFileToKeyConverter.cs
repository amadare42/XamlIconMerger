namespace XamlIconMerger.Filesystem
{
    public interface IFileToKeyConverter
    {
        string GetKey(string path);
    }
}