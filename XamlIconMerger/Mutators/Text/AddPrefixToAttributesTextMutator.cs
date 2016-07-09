using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XamlIconMerger.Mutators.Text
{
    public class AddPrefixToAttributesTextMutator : ITextMutator
    {
        private readonly string key;
        private readonly string prefix;
        private readonly string searchKey;

        public AddPrefixToAttributesTextMutator(string searchKey, string prefix, string key)
        {
            this.searchKey = searchKey;
            this.prefix = prefix;
            this.key = key;
        }

        public AddPrefixToAttributesTextMutator(string searchKey, string prefix)
        {
            this.searchKey = searchKey;
            this.prefix = prefix;
            this.key = searchKey;
        }

        public string Mutate(string text, IElementSource source)
        {
            var attributeRegex = new Regex($"{this.searchKey}=\"");
            var matches = attributeRegex.Matches(text).Cast<Match>().ToList();
            if (matches.Count == 0)
                return text;
            var builder = new StringBuilder();

            var index = 0;
            string appendString = $"{prefix}:{key}=\"";
            foreach (var m in matches)
            {
                if (index < m.Index)
                {
                    builder.Append(text.Substring(index, m.Index - index));
                }
                builder.Append(appendString);
                index = m.Index + m.Length;
            }
            var last = matches.Last();
            var lastIndex = last.Index + last.Length;
            if (lastIndex < text.Length - 1)
            {
                builder.Append(text.Substring(lastIndex));
            }
            return builder.ToString();
        }
    }
}