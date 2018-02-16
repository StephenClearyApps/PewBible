using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class IfbWeb
    {
        public static List<Verse> Import(IReadOnlyList<string> text)
        {
            var result = new List<Verse>();
            string currentBook = null;
            var currentChapter = 0;
            foreach (var line in text.Where(x => !string.IsNullOrEmpty(x)).ConvertPsalmToChapters().MergePreColophones())
            {
                if (line.StartsWith("CHAPTER"))
                {
                    currentChapter = int.Parse(line.Substring(8));
                }
                else if (line.ToUpper() == line)
                {
                    currentBook = line;
                }
                else if (!char.IsDigit(line[0]))
                {
                }
                else
                {
                    var spaceIndex = line.IndexOf(' ');
                    result.Add(new Verse(currentBook, currentChapter, int.Parse(line.Substring(0, spaceIndex)), line.Substring(spaceIndex + 1).Replace('#', ' ').Replace('[', ' ').Replace(']', ' ')));
                }
            }

            return result;
        }

        private static IEnumerable<string> ConvertPsalmToChapters(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
                if (line.StartsWith("PSALM"))
                    yield return line.Replace("PSALM", "CHAPTER");
                else
                    yield return line;
        }

        private static IEnumerable<string> MergePreColophones(this IEnumerable<string> lines)
        {
            var preText = string.Empty;
            foreach (var line in lines)
            {
                if (line.ToUpper() == line && !Ps119.Contains(line))
                {
                    // Pass through post-colophones.
                    if (preText != string.Empty)
                    {
                        yield return preText;
                        preText = string.Empty;
                    }
                    yield return line;
                }
                else if (char.IsDigit(line[0]))
                {
                    if (preText != string.Empty)
                    {
                        yield return preText + " " + line;
                        preText = string.Empty;
                    }
                    else
                        yield return line;
                }
                else
                {
                    if (preText != string.Empty)
                        throw new InvalidOperationException("Multiple colophones.");
                    preText = line;
                }
            }

            if (preText != string.Empty)
                throw new InvalidOperationException("Ending colophon " + preText);
        }

        private static readonly string[] Ps119 = new[]
        {
            "ALEPH.",
            "BETH.",
            "GIMEL.",
            "DALETH.",
            "HE.",
            "VAU.",
            "ZAIN.",
            "CHETH.",
            "TETH.",
            "JOD.",
            "CAPH.",
            "LAMED.",
            "MEM.",
            "NUN.",
            "SAMECH.",
            "AIN.",
            "PE.",
            "TZADDI.",
            "KOPH.",
            "RESH.",
            "SCHIN.",
            "TAU.",
        };
    }
}
