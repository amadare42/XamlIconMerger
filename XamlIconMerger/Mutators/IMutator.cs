namespace XamlIconMerger.Mutators
{
    public interface IMutator<TMutable>
    {
        TMutable Mutate(TMutable value, IElementSource source);
    }
}