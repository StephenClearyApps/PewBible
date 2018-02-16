using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public static class Export
    {
        public static string PunctuatedText(this IEnumerable<string> words)
        {
            var sb = new StringBuilder();
            var first = true;
            var lastWasOpenPunct = false;
            foreach (var word in words)
            {
                if (first)
                {
                    sb.Append(word);
                    first = false;
                }
                else
                {
                    if (word.Length != 1 || char.IsLetter(word[0]) || word == "(" || word == "[" || word == "<")
                    {
                        if (!lastWasOpenPunct)
                            sb.Append(" ");
                        sb.Append(word);
                    }
                    else if (word == "`")
                    {
                        sb.Append("'s");
                    }
                    else
                    {
                        sb.Append(word);
                    }
                }
                lastWasOpenPunct = word == "(" || word == "[" || word == "<" || word == "-";
            }
            return sb.ToString();
        }

        public static string UnpunctuatedText(this IEnumerable<string> words)
        {
            var sb = new StringBuilder();
            var first = true;
            foreach (var word in words)
            {
                if (first)
                {
                    sb.Append(word);
                    first = false;
                }
                else if (word == "`")
                {
                    sb.Append("s");
                }
                else if (char.IsLetter(word[0]))
                {
                    sb.Append(" " + word);
                }
            }
            return sb.ToString();
        }

        public static Structure Structure(this IReadOnlyList<Verse> verses)
        {
            var result = new Structure();
            Book book = null;
            Chapter chapter = null;
            for (int verseNumber = 0; verseNumber != verses.Count; ++verseNumber)
            {
                var verse = verses[verseNumber];
                if (book == null || book.Name != verse.Book)
                {
                    book = new Book(verse.Book, result.Books.Count) { BeginVerse = verseNumber, EndVerse = verseNumber + 1 };
                    result.Books.Add(book);
                    chapter = new Chapter(verse.Chapter - 1) { BeginVerse = verseNumber, EndVerse = verseNumber + 1 };
                    book.Chapters.Add(chapter);
                }
                else if (chapter.Index != verse.Chapter - 1)
                {
                    chapter = new Chapter(verse.Chapter - 1) { BeginVerse = verseNumber, EndVerse = verseNumber + 1 };
                    book.Chapters.Add(chapter);
                }
                else
                {
                    book.EndVerse = verseNumber + 1;
                    chapter.EndVerse = verseNumber + 1;
                }
            }
            foreach (var b in result.Books)
                b.NormalizeName();
            return result;
        }
    }
}
