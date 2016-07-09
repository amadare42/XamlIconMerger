using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XamlIconMerger.Mutators.Text
{
    public class RegexpCleanerMutator : ITextMutator
    {
        private readonly string keyElement;

        public RegexpCleanerMutator(string keyElement = null)
        {
            this.keyElement = keyElement;
        }

        public string Mutate(string fileContent, IElementSource source = null)
        {
            var commentsRegex = new Regex("<!--.*?-->(\r\n)*", RegexOptions.Multiline);
            var namespacesRegex = new Regex(" *(\r\n)* *xmlns[^=]*=\"[^\"]*\"(\r\n)*", RegexOptions.Multiline);
            var roundRegex = new Regex("(Width|Height)=\"\\d+(\\.000)\"", RegexOptions.Multiline);

            var commentMatches = commentsRegex.Matches(fileContent);
            var namespacesMatches = namespacesRegex.Matches(fileContent);
            var roundMatches = roundRegex.Matches(fileContent);

            var matchIds = this.convertToMatchIdentifiers(commentMatches, namespacesMatches);
            matchIds = matchIds.Concat(roundMatches.Cast<Match>().Select(match => new MatchIdentifier(match, 2)));

            var cleanedStringBuilder = this.cleanFromMatches(fileContent, matchIds);
            //this.AddKey(cleanedStringBuilder, name);
            var cleanedString = cleanedStringBuilder.ToString();

            return cleanedString;
        }

        private void AddKey(StringBuilder cleanedStringBuilder, string name)
        {
            if (this.keyElement == null)
                return;

            var cleaned = cleanedStringBuilder.ToString();
            var viewboxIndex = cleaned.IndexOf(this.keyElement, StringComparison.InvariantCulture);
            if (viewboxIndex > 0)
            {
                cleanedStringBuilder.Insert(viewboxIndex + this.keyElement.Length,
                    $" x:Key=\"{name}\"");
            }
            else
            {
                throw new InvalidOperationException($"Cannot find '{this.keyElement}' tag.");
            }
        }

        private IEnumerable<MatchIdentifier> convertToMatchIdentifiers(params MatchCollection[] collections)
        {
            var enumerable = Enumerable.Empty<MatchIdentifier>();
            foreach (var matchCollection in collections)
            {
                var converted = matchCollection.Cast<Match>().Select(this.ConvertMatchToMatchIdentifier);
                enumerable = enumerable.Concat(converted);
            }
            return enumerable;
        }

        private MatchIdentifier ConvertMatchToMatchIdentifier(Match m)
        {
            return new MatchIdentifier(m);
        }

        private StringBuilder cleanFromMatches(string text, IEnumerable<MatchIdentifier> match)
        {
            var matchList = match.ToList();
            if (matchList.Count == 0)
            {
                return new StringBuilder(text);
            }
            matchList.Sort((m1, m2) => m1.Index - m2.Index);

            var index = 0;
            var builder = new StringBuilder();
            foreach (var m in matchList)
            {
                if (index < m.Index)
                {
                    builder.Append(text.Substring(index, m.Index - index));
                }
                index = m.LastIndex;
            }

            var last = matchList.Last();
            if (last.LastIndex < text.Length - 1)
            {
                builder.Append(text.Substring(last.LastIndex));
            }

            return builder;
        }
    }
}