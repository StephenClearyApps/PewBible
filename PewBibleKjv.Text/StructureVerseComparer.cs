using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Text
{
    public sealed class StructureVerseComparer : IComparer
    {
        public static StructureVerseComparer Instance { get; } = new StructureVerseComparer();

        public int Compare(object x, object y)
        {
            var range = (IVerseRange)x;
            var absoluteVerseNumber = (int)y;
            if (range.EndVerse <= absoluteVerseNumber)
                return -1;
            if (range.BeginVerse > absoluteVerseNumber)
                return 1;
            return 0;
        }
    }
}
