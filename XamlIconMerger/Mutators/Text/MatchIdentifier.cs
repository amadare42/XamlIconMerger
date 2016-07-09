using System.Text.RegularExpressions;

namespace XamlIconMerger.Mutators.Text
{
    internal class MatchIdentifier
    {
        public readonly int Index;
        public readonly int Length;

        public MatchIdentifier(int index, int length)
        {
            this.Index = index;
            this.Length = length;
        }

        public MatchIdentifier(Match match) : this(match.Index, match.Length)
        {
            //empty
        }

        public MatchIdentifier(Match match, int group) : this(match.Groups[group].Index, match.Groups[group].Length)
        {
            //empty
        }

        public int LastIndex
        {
            get { return this.Index + this.Length; }
        }
    }
}