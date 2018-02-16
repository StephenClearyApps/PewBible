using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImportAndCompare
{
    public sealed class Structure
    {
        public Structure()
        {
            Books = new List<Book>();
        }

        [JsonProperty(PropertyName = "structure")]
        public List<Book> Books { get; private set; }
    }

    public sealed class Book
    {
        public Book(string name, int index)
        {
            Name = name;
            Index = index;
            Chapters = new List<Chapter>();
        }

        [JsonProperty(PropertyName = "n")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "i")]
        public int Index { get; private set; }

        [JsonProperty(PropertyName = "c")]
        public List<Chapter> Chapters { get; private set; }

        [JsonProperty(PropertyName = "b")]
        public int BeginVerse { get; set; }

        [JsonProperty(PropertyName = "e")]
        public int EndVerse { get; set; }

        public void NormalizeName()
        {
            Name = CanonicalBookNames[Index];
        }

        private static readonly string[] CanonicalBookNames = new[]
        {
            "Genesis",
            "Exodus",
            "Leviticus",
            "Numbers",
            "Deuteronomy",
            "Joshua",
            "Judges",
            "Ruth",
            "1 Samuel",
            "2 Samuel",
            "1 Kings",
            "2 Kings",
            "1 Chronicles",
            "2 Chronicles",
            "Ezra",
            "Nehemiah",
            "Esther",
            "Job",
            "Psalms",
            "Proverbs",
            "Ecclesiastes",
            "Song of Solomon",
            "Isaiah",
            "Jeremiah",
            "Lamentations",
            "Ezekiel",
            "Daniel",
            "Hosea",
            "Joel",
            "Amos",
            "Obadiah",
            "Jonah",
            "Micah",
            "Nahum",
            "Habakkuk",
            "Zephaniah",
            "Haggai",
            "Zechariah",
            "Malachi",
            "Matthew",
            "Mark",
            "Luke",
            "John",
            "Acts",
            "Romans",
            "1 Corinthians",
            "2 Corinthians",
            "Galatians",
            "Ephesians",
            "Philippians",
            "Colossians",
            "1 Thessalonians",
            "2 Thessalonians",
            "1 Timothy",
            "2 Timothy",
            "Titus",
            "Philemon",
            "Hebrews",
            "James",
            "1 Peter",
            "2 Peter",
            "1 John",
            "2 John",
            "3 John",
            "Jude",
            "Revelation"
        };
    }

    public sealed class Chapter
    {
        public Chapter(int index)
        {
            Index = index;
        }

        [JsonProperty(PropertyName = "i")]
        public int Index { get; private set; }

        [JsonProperty(PropertyName = "b")]
        public int BeginVerse { get; set; }

        [JsonProperty(PropertyName = "e")]
        public int EndVerse { get; set; }
    }
}
