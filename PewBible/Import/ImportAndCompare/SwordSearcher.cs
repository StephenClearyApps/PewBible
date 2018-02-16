using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class SwordSearcher
    {
        public static List<Verse> Import(IReadOnlyList<string> text)
        {
            var bookChapterVerseRegex = new Regex("([0-9A-Za-z ]+) ([0-9]+):([0-9]+)");
            var result = new List<Verse>();
            string currentBook = null;
            var currentChapter = 0;
            foreach (var line in text.Where(x => !string.IsNullOrEmpty(x)))
            {
                var match = bookChapterVerseRegex.Match(line);
                int verseNumber;
                string verseText;
                if (!match.Success)
                {
                    var firstSpace = line.IndexOf(' ');
                    verseNumber = int.Parse(line.Substring(0, firstSpace));
                    verseText = line.Substring(firstSpace + 1);
                }
                else
                {
                    currentBook = match.Groups[1].Value;
                    currentChapter = int.Parse(match.Groups[2].Value);
                    verseNumber = int.Parse(match.Groups[3].Value);
                    verseText = line.Substring(match.Length + 1);
                }

                result.Add(new Verse(currentBook, currentChapter, verseNumber, verseText.Replace("--", "—").Replace("LORD'S", "LORD's")));
            }

            return result;
        }
    }
}
