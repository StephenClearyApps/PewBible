using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
{
    public sealed class TextService: IReadOnlyList<Location>
    {
        public IEnumerator<Location> GetEnumerator()
        {
            for (var i = 0; i != Count; ++i)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => Structure.Books.Last().Chapters.Last().EndVerse;

        public Location this[int index] => Location.Create(index);
    }
}
