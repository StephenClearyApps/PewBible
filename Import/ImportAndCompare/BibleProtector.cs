using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class BibleProtector
    {
        public static List<Verse> Import(IReadOnlyList<string> text)
        {
            var bookChapterVerseRegex = new Regex("([0-9A-Za-z ]+) ([0-9]+):([0-9]+)");
            var result = new List<Verse>();
            foreach (var line in text)
            {
                var match = bookChapterVerseRegex.Match(line);
                var verse = line.Substring(match.Length + 1).Replace("’", "'");
                if (verse.Contains("<<[") && verse.Contains("]>>"))
                    verse = verse.Replace("<<[", "<").Replace("]>>", ">");
                else
                    verse = verse.Replace("<<", "<").Replace(">>", ">");
                result.Add(new Verse(match.Groups[1].Value, int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), verse));
            }

            return result;
        }
    }
}
