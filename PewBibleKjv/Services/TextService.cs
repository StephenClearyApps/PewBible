using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PewBibleKjv.Text;

namespace PewBibleKjv.Services
{
    public sealed class TextService: IReadOnlyList<Location>
    {
        public static TextService Instance { get; } = new TextService();

        public IEnumerator<Location> GetEnumerator()
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

        public Location this[int index]
        {
            get
            {
                Debug.WriteLine("Retrieving " + index);
                return Location.Create(index);
            }
        }
    }
}
