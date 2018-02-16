using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportAndCompare
{
    public sealed class Verse
    {
        public Verse(string book, int chapter, int verseNumber, IList<string> words)
        {
            if (string.IsNullOrWhiteSpace(book))
                throw new InvalidOperationException("Invalid book " + book);
            if (chapter <= 0)
                throw new InvalidOperationException("Invalid chapter " + chapter);
            if (verseNumber <= 0)
                throw new InvalidOperationException("Invalid verse number " + verseNumber);
            if (words.Count == 0 || words.All(string.IsNullOrWhiteSpace))
                throw new InvalidOperationException("Invalid text");
            Book = book;
            Chapter = chapter;
            VerseNumber = verseNumber;
            Words = words;
            foreach (var word in Words)
            {
                if (word.Length != 1 && !word.All(char.IsLetter))
                    throw new InvalidOperationException("Unrecognized punctuation " + word);
            }
        }

        public Verse(string book, int chapter, int verseNumber, string text)
            : this(book, chapter, verseNumber, ParseText(text).ToArray())
        {
        }

        public string Book { get; private set; }
        public int Chapter { get; private set; }
        public int VerseNumber { get; private set; }

        public IList<string> Words { get; private set; }

        private static IEnumerable<string> ParseText(string text)
        {
            var endPunct = new List<string>();
            var mixedCase = new Regex("[a-z][A-Z]");
            foreach (var textWord in text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var word = textWord;
                endPunct.Clear();

                // Strip starting punctuation from word.
                while (word.StartsWith("(") || word.StartsWith("[") || word.StartsWith("<"))
                {
                    yield return word.Substring(0, 1);
                    word = word.Substring(1).TrimStart();
                }

                // Strip ending punctuation from word.
                while (word.EndsWith(")") || word.EndsWith(".") || word.EndsWith(",") || word.EndsWith(";") || word.EndsWith(":") || word.EndsWith("?") || word.EndsWith("!") || word.EndsWith("—") || word.EndsWith("]") || word.EndsWith(">") || word.EndsWith("'s") || word.EndsWith("'"))
                {
                    if (word.EndsWith("'s"))
                    {
                        endPunct.Insert(0, "`");
                        word = word.Substring(0, word.Length - 2).TrimEnd();
                    }
                    else
                    {
                        endPunct.Insert(0, word.Substring(word.Length - 1));
                        word = word.Substring(0, word.Length - 1).TrimEnd();
                    }
                }

                // Hyphenate mixed-case words.
                var matches = mixedCase.Matches(word);
                if (matches.Count != 0)
                    word = mixedCase.Replace(word, x => x.Value[0] + "-" + x.Value[1]);

                // Split hyphenated words.
                var hyphenIndex = word.IndexOf('-');
                if (hyphenIndex != -1)
                {
                    yield return word.Substring(0, hyphenIndex);
                    yield return "-";
                    word = word.Substring(hyphenIndex + 1);
                }

                if (word != string.Empty)
                    yield return word;
                foreach (var punct in endPunct)
                    yield return punct;
            }
        }
    }
}
