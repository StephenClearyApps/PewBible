using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public static class VerseHelper
    {
        public static Location Find(string bookName, int chapterNumber = 1, int verseNumber = 1)
        {
            var book = Structure.Books.First(x => x.Name == bookName);
            var chapter = book.Chapters[chapterNumber - 1];
            return new Location(book, chapter, chapter.BeginVerse + verseNumber - 1);
        }
    }
}
