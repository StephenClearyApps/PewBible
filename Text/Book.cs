using System;
using System.Collections.Generic;

namespace PewBible.Text
{
    public sealed class Book
    {
        public string Name { get; set; }

        public int Index { get; set; }

        public List<Chapter> Chapters { get; set; }

        public int BeginVerse { get; set; }

        public int EndVerse { get; set; }
    }
}
