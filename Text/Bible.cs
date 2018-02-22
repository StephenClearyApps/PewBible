using System;
using System.Collections.Generic;
using System.Text;

namespace PewBible.Text
{
    public static class Bible
    {
        public static IEnumerable<string> VerseWords(int verseNumber)
        {
            var beginEndBytes = new byte[sizeof(int) * 2];
            Data.VerseIndex.Position = verseNumber * sizeof(int);
            Data.VerseIndex.Read(beginEndBytes, 0, sizeof(int) * 2);
            var begin = BitConverter.ToInt32(beginEndBytes, 0);
            var end = BitConverter.ToInt32(beginEndBytes, sizeof(int));

            var verseDataBytes = new byte[sizeof(ushort) * (end - begin)];
            Data.Verses.Position = sizeof(ushort) * begin;
            Data.Verses.Read(verseDataBytes, 0, verseDataBytes.Length);
            var verseData = new ushort[end - begin];
            Buffer.BlockCopy(verseDataBytes, 0, verseData, 0, verseDataBytes.Length);

            foreach (var value in verseData)
            {
                // Look up the word in either the words or punctuation tables, and adjust casing if necessary.
                var wordIndex = value & 0x3FFF;
                var isPunctuation = wordIndex >= Constants.Words.Length;
                if (isPunctuation)
                {
                    yield return Constants.Punctuation[wordIndex - Constants.Words.Length];
                }
                else
                {
                    var word = Constants.Words[wordIndex];
                    var wordFlags = value >> 14;
                    if (wordFlags == 1)
                        word = word.ToUpperInvariant();
                    else if (wordFlags == 2)
                        word = char.ToUpperInvariant(word[0]) + word.Substring(1);
                    else if (wordFlags == 3)
                        word = char.ToUpperInvariant(word[0]) + char.ToUpperInvariant(word[1]) + word.Substring(2);
                    yield return word;
                }
            }
        }
    }
}
