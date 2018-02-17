using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PewBibleKjv.ViewModels
{
    public sealed class TestData: IReadOnlyList<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            for (var i = 0; i != Count; ++i)
            {
                Debug.WriteLine("Enumerating " + i);
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => 10000;

        public int this[int index]
        {
            get
            {
                Debug.WriteLine("Retrieving " + index);
                return index;
            }
        }
    }
}
