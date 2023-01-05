using System.Text.RegularExpressions;

namespace TwitterSentiments.Utilities
{
    public static class StringFunctions
    {
        private static Regex hashTagRegex = new Regex(Constants.HashTagPattern);

        public static string[] ExtractTwitterTagsFromText(string textToSearch)
        {
            if (string.IsNullOrEmpty(textToSearch))
            {
                return Array.Empty<string>();
            }

            MatchCollection matchedTags = hashTagRegex.Matches(textToSearch);

            return matchedTags
                .Where(t => !string.IsNullOrEmpty(t.Value))
                .Select(x => x.Value)
                .ToArray();
        }
    }
}