using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
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

        public int Count
        {
            get
            {
                var lastBook = Structure.Books[Structure.Books.Length - 1];
                var lastChapter = lastBook.Chapters[lastBook.Chapters.Length - 1];
                return lastChapter.EndVerse;
            }
        }

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
