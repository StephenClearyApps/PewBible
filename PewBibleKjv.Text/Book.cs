using System;
using System.Collections.Generic;

namespace PewBibleKjv.Text
{
    public sealed class Book
    {
        public Book(int index, string name, int beginVerse, int endVerse, IReadOnlyList<Chapter> chapters)
        {
            Index = index;
            Name = name;
            BeginVerse = beginVerse;
            EndVerse = endVerse;
            Chapters = chapters;
        }

        public int Index { get; }

        public string Name { get; }

        public int BeginVerse { get; }

        public int EndVerse { get; }

        public IReadOnlyList<Chapter> Chapters { get; }
    }
}
