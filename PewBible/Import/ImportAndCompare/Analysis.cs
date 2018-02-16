using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class Analysis
    {
        public static IEnumerable<string> Books(this IReadOnlyList<Verse> verses)
        {
            string last = null;
            foreach (var verse in verses)
            {
                if (verse.Book == last)
                    continue;
                last = verse.Book;
                yield return verse.Book;
            }
        }

        public static int NumberOfBooks(this IReadOnlyList<Verse> verses)
        {
            return verses.Select(x => x.Book).Distinct().Count();
        }

        public static int NumberOfPunctuation(this IReadOnlyList<Verse> verses)
        {
            return verses.SelectMany(x => x.Words).Where(w => !char.IsLetter(w[0])).Distinct().Count();
        }

        public static int NumberOfWords(this IReadOnlyList<Verse> verses)
        {
            return verses.SelectMany(x => x.Words).Where(w => char.IsLetter(w[0])).Distinct().Count();
        }

        public static int NumberOfCaseInsensitiveWords(this IReadOnlyList<Verse> verses)
        {
            return verses.SelectMany(x => x.Words).Where(w => char.IsLetter(w[0])).Distinct(StringComparer.InvariantCultureIgnoreCase).Count();
        }

        public static void VerifyPunctuation(this IReadOnlyList<Verse> verses)
        {
            var inParen = false;
            var inItalics = false;
            var inColophon = false;
            foreach (var verse in verses)
            {
                foreach (var word in verse.Words)
                {
                    if (word[0] == '(')
                    {
                        if (inParen)
                            throw new InvalidOperationException("Nested ( in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inParen = true;
                    }
                    else if (word[0] == ')')
                    {
                        if (!inParen)
                            throw new InvalidOperationException("Unmatched ) in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inParen = false;
                    }
                    else if (word[0] == '[')
                    {
                        if (inItalics)
                            throw new InvalidOperationException("Nested [ in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inItalics = true;
                    }
                    else if (word[0] == ']')
                    {
                        if (!inItalics)
                            throw new InvalidOperationException("Unmatched ] in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inItalics = false;
                    }
                    else if (word[0] == '<')
                    {
                        if (inColophon)
                            throw new InvalidOperationException("Nested < in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inColophon = true;
                    }
                    else if (word[0] == '>')
                    {
                        if (!inColophon)
                            throw new InvalidOperationException("Unmatched > in " + verse.Book + " " + verse.Chapter + ":" + verse.VerseNumber);
                        inColophon = false;
                    }
                }
            }

            if (inParen)
                throw new InvalidOperationException("Unmatched (");
            if (inItalics)
                throw new InvalidOperationException("Unmatched [");
            if (inColophon)
                throw new InvalidOperationException("Unmatched <");
        }
    }
}
