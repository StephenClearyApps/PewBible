using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Text
{
    public sealed class Chapter
    {
        public Chapter(int beginVerse, int endVerse)
        {
            BeginVerse = beginVerse;
            EndVerse = endVerse;
        }

        public int BeginVerse { get; }

        public int EndVerse { get; }
    }
}
