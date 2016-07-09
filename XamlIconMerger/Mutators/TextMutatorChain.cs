namespace XamlIconMerger.Mutators
{
    public class TextMutatorChain : MutatorChain<string>, ITextMutator
    {
        public TextMutatorChain(params IMutator<string>[] mutators) : base(mutators)
        {
        }
    }
}