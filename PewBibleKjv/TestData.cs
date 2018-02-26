using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PewBibleKjv.ViewModels
{
    public sealed class TestData: IReadOnlyList<string>
    {
        public IEnumerator<string> GetEnumerator()
        {
            for (var i = 0; i != Count; ++i)
            {
                Debug.WriteLine("Enumerating " + i);
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => 31102;

        public string this[int index]
        {
            get
            {
                Debug.WriteLine("Retrieving " + index);
                return PewBible.Text.Bible.FormattedVerse(index).Text;
            }
        }
    }
}
