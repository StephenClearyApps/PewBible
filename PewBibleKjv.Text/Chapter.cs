using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Text
{
    public sealed class Chapter
    {
        public Chapter(int index, int beginVerse, int endVerse)
        {
            Index = index;
            BeginVerse = beginVerse;
            EndVerse = endVerse;
        }

        public int Index { get; }

        public int BeginVerse { get; }

        public int EndVerse { get; }
    }
}
