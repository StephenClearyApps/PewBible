using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class Location
    {
        public Location(Book book, Chapter chapter, int absoluteVerseNumber)
        {
            Book = book;
            Chapter = chapter;
            AbsoluteVerseNumber = absoluteVerseNumber;
        }

        public int AbsoluteVerseNumber { get; }
        public Book Book { get; }
        public Chapter Chapter { get; }

        public int Verse => AbsoluteVerseNumber - Chapter.BeginVerse + 1;
        public int ChapterNumber => Chapter.Index + 1;
        public string ChapterHeadingText => Book.Name + " " + ChapterNumber;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => Book.Name + " " + ChapterNumber + ":" + Verse;

        public static Location Create(int absoluteVerseNumber)
        {
            var bookIndex = Array.BinarySearch(Structure.Books, absoluteVerseNumber, StructureVerseComparer.Instance);
            var book = Structure.Books[bookIndex];
            var chapterIndex = Array.BinarySearch(book.Chapters, absoluteVerseNumber, StructureVerseComparer.Instance);
            var chapter = book.Chapters[chapterIndex];
            return new Location(book, chapter, absoluteVerseNumber);
        }

        public Location PreviousChapter()
        {
            var book = Book;
            var chapter = Chapter;
            if (chapter.Index == 0)
            {
                if (book.Index != 0)
                {
                    book = Structure.Books[book.Index - 1];
                    chapter = book.Chapters.Last();
                }
            }
            else
            {
                chapter = book.Chapters[chapter.Index - 1];
            }
            return new Location(book, chapter, chapter.BeginVerse);
        }

        public Location NextChapter()
        {
            var book = Book;
            var chapter = Chapter;
            if (chapter.Index == book.Chapters.Length - 1)
            {
                if (book.Index != Structure.Books.Length - 1)
                {
                    book = Structure.Books[book.Index + 1];
                    chapter = book.Chapters.First();
                }
            }
            else
            {
                chapter = book.Chapters[chapter.Index + 1];
            }
            return new Location(book, chapter, chapter.BeginVerse);
        }
    }
}
