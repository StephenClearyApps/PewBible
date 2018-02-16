using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class ProjectGutenbergA
    {
        public static List<Verse> Import(IReadOnlyList<string> text)
        {
            var chapterVerseRegex = new Regex("([0-9]+):([0-9]+)");
            var result = new List<Verse>();
            string currentBook = null;
            foreach (var line in SplitVerses(CombineMultilineVerses(text)))
            {
                if (!char.IsDigit(line[0]))
                {
                    currentBook = line;
                }
                else
                {
                    var match = chapterVerseRegex.Match(line);
                    // Ensure some punctuation has spaces after.
                    var verse = line.Substring(match.Length + 1);
                    verse = verse.Replace(":", ": ");
                    result.Add(new Verse(currentBook, int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), verse));
                }
            }
            return result;
        }

        private static IEnumerable<string> SplitVerses(IEnumerable<string> verses)
        {
            var chapterVerseRegex = new Regex("([0-9]+):([0-9]+) [^0-9]+");
            foreach (var verse in verses)
            {
                var matches = chapterVerseRegex.Matches(verse);
                if (matches.Count == 0)
                    yield return verse;
                else
                {
                    foreach (Match match in matches)
                    {
                        yield return match.Value;
                    }
                }
            }
        }

        private static IEnumerable<string> CombineMultilineVerses(IEnumerable<string> text)
        {
            var result = string.Empty;
            foreach (var line in text)
            {
                if (line == string.Empty)
                {
                    if (result != string.Empty)
                    {
                        yield return result;
                        result = string.Empty;
                    }
                }
                else
                {
                    if (result == string.Empty)
                        result = line;
                    else
                        result += " " + line;
                }
            }
            if (result != string.Empty)
                yield return result;
        }
    }
}
