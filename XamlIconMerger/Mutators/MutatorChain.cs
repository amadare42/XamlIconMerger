namespace XamlIconMerger.Mutators
{
    public class MutatorChain<TMutable> : IMutator<TMutable>
    {
        private readonly IMutator<TMutable>[] mutators;

        public MutatorChain(params IMutator<TMutable>[] mutators)
        {
            this.mutators = mutators;
        }

        public TMutable Mutate(TMutable text, IElementSource source)
        {
            foreach (var mutator in this.mutators)
            {
                text = mutator.Mutate(text, source);
            }

            return text;
        }
    }
}