using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class ProjectGutenbergB
    {
        public static List<Verse> Import(IReadOnlyList<string> text)
        {
            var result = new List<Verse>();
            foreach (var line in text.Verses())
            {
                result.Add(new Verse(line.Substring(0, 2), int.Parse(line.Substring(3, 3)), int.Parse(line.Substring(7, 3)), line.Substring(11)));
            }

            return result;
        }

        private static IEnumerable<string> Verses(this IEnumerable<string> lines)
        {
            string text = null;
            foreach (var line in lines.Where(x => !string.IsNullOrEmpty(x) && !x.StartsWith("Book")))
            {
                if (char.IsDigit(line[0]))
                {
                    if (text != null)
                    {
                        yield return text;
                    }
                    text = line;
                }
                else
                {
                    text += " " + line.Trim();
                }
            }
            yield return text;
        }
    }
}
