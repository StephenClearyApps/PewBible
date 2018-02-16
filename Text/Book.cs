using System;
using System.Collections.Generic;

namespace PewBible.Text
{
    public sealed class Book
    {
        public Book(string name, int beginVerse, int endVerse, IReadOnlyList<Chapter> chapters)
        {
            Name = name;
            BeginVerse = beginVerse;
            EndVerse = endVerse;
            Chapters = chapters;
        }

        public string Name { get; }

        public int BeginVerse { get; }

        public int EndVerse { get; }

        public IReadOnlyList<Chapter> Chapters { get; }
    }
}
