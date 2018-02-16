using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ImportAndCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessBibleProtector();
                //ProcessProjectGutenbergB();
                //ProcessKjvRaw();
                //ProcessIfbWeb();
                //ProcessSwordSearcher();
                //ProcessProjectGutenbergA();
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        public static void ProcessBibleProtector()
        {
            var verses = BibleProtector.Import(File.ReadAllLines(@"..\..\..\BibleProtector\TEXT-PCE.txt", Encoding.GetEncoding(1252)));
            verses.VerifyPunctuation();
            Export(verses, "PCE");
            Stats(verses);
            //foreach (var word in verses.SelectMany(x => x.Words).Where(x => char.IsLetter(x[0])).Distinct())
            //{
            //    if (!word.All(char.IsUpper) && !word.All(char.IsLower) && (!char.IsUpper(word[0]) || !word.Skip(1).All(char.IsLower)))
            //        Console.WriteLine(word);
            //}
        }

        public static void ProcessKjvRaw()
        {
            var verses = KjvRaw.Import(File.ReadAllLines(@"..\..\..\Textfile 930105\kjv.raw.txt"));
            Export(verses, "RAW");
            Stats(verses);
        }

        public static void ProcessIfbWeb()
        {
            var verses = IfbWeb.Import(File.ReadAllLines(@"..\..\..\printkjv.ifbweb.com\AV1611Bible.txt"));
            Export(verses, "IFB");
            Stats(verses);
        }

        public static void ProcessProjectGutenbergA()
        {
            var verses = ProjectGutenbergA.Import(File.ReadAllLines(@"..\..\..\Gutenberg-A\pg10.txt"));
            Export(verses, "PGA");
            Stats(verses);
        }

        public static void ProcessProjectGutenbergB()
        {
            var verses = ProjectGutenbergA.Import(File.ReadAllLines(@"..\..\..\Gutenberg-B\bible13.txt"));
            Export(verses, "PGB");
            Stats(verses);
        }

        public static void ProcessSwordSearcher()
        {
            var verses = SwordSearcher.Import(File.ReadAllLines(@"..\..\..\SS5\SS5.txt"));
            Export(verses, "SS5");
            Stats(verses);
        }

        public static void Export(IReadOnlyList<Verse> verses, string prefix)
        {
            checked
            {
                File.WriteAllLines(prefix + ".standard.txt", verses.Select(x => x.Words.PunctuatedText()));
                File.WriteAllLines(prefix + ".simple.txt", verses.Select(x => x.Words.Select(y => y.ToLower()).UnpunctuatedText()));
                //File.WriteAllLines(prefix + ".full.txt", verses.Select(x => x.Book + " " + x.Chapter + ":" + x.VerseNumber + " " + x.Words.PunctuatedText()));

                var utf8 = new UTF8Encoding(false);
                var words = verses.SelectMany(x => x.Words).Where(x => char.IsLetter(x[0])).Select(x => x.ToLower()).Distinct().OrderBy(x => x).ToList();
                var punctuation = verses.SelectMany(x => x.Words).Where(x => !char.IsLetter(x[0])).Distinct().OrderBy(x => x).ToList();

                var punctuationJson = JsonConvert.SerializeObject(punctuation.Select(x => x.Replace("`", "'s")), Formatting.None);
                File.WriteAllText("punctuation.js", "define(function () { return " + punctuationJson + "; });", utf8);
                File.WriteAllText("Constants.Punctuation.cs", "namespace PewBible {\npublic static partial class Constants {\npublic static string[] Punctuation = " + CSharpSerialize(punctuation.Select(x => x.Replace("`", "'s"))) + ";\n}\n}");

                var wordsJson = JsonConvert.SerializeObject(words, Formatting.None);
                File.WriteAllText("words.js", "define(function () { return " + wordsJson + "; });", utf8);
                File.WriteAllText("Constants.Words.cs", "namespace PewBible {\npublic static partial class Constants {\npublic static string[] Words = " + CSharpSerialize(words) + ";\n}\n}");

                var structure = verses.Structure();
                var jsonStructure = JsonConvert.SerializeObject(structure.Books, Formatting.None);
                File.WriteAllText("structure.js", "define(function () { return " + jsonStructure + "; });", utf8);
                File.WriteAllText("Structure.cs", CSharpSerialize(structure));

                var verseIndex = new List<int>();
                using (var dataFile = new FileStream("verses.dat", FileMode.Create))
                {
                    foreach (var verse in verses)
                    {
                        verseIndex.Add((int)(dataFile.Position / 2));
                        foreach (var word in verse.Words)
                        {
                            var wordIndex = words.BinarySearch(word.ToLower());
                            if (wordIndex >= 0)
                            {
                                int wordFlags = 0;
                                if (word.All(char.IsUpper))
                                    wordFlags = 1;
                                else if (char.IsUpper(word[0]) && word.Skip(1).All(char.IsLower))
                                    wordFlags = 2;
                                else if (char.IsUpper(word[0]) && char.IsUpper(word[1]) && word.Skip(2).All(char.IsLower))
                                    wordFlags = 3;
                                else if (!word.All(char.IsLower))
                                    throw new InvalidOperationException("Odd word " + word);
                                dataFile.Write(BitConverter.GetBytes((ushort)((wordFlags << 14) | wordIndex)), 0, 2);
                            }
                            else
                            {
                                var punctIndex = punctuation.BinarySearch(word);
                                if (punctIndex < 0)
                                    throw new InvalidOperationException("Impossible things are happening every day!");
                                dataFile.Write(BitConverter.GetBytes((ushort)(words.Count + punctIndex)), 0, 2);
                            }
                        }
                    }
                    verseIndex.Add((int)(dataFile.Position / 2));
                }

                var verseIndexJson = JsonConvert.SerializeObject(verseIndex, Formatting.None);
                File.WriteAllText("verseIndex.js", "define(function () { return " + verseIndexJson + "; });", utf8);

                var concordance = new List<int>[words.Count];
                for (var i = 0; i != concordance.Length; ++i)
                    concordance[i] = new List<int>();
                for (var i = 0; i != verses.Count; ++i)
                {
                    var verse = verses[i];
                    foreach (var word in verse.Words)
                    {
                        var wordIndex = words.BinarySearch(word.ToLower());
                        if (wordIndex < 0)
                            continue;
                        concordance[wordIndex].Add(i);
                    }
                }

                var cWordIndex = new List<int>();
                using (var dataFile = new FileStream("concordance.dat", FileMode.Create))
                {
                    foreach (var cWord in concordance)
                    {
                        cWordIndex.Add((int)(dataFile.Position / 2));
                        foreach (var verseNumber in cWord)
                        {
                            dataFile.Write(BitConverter.GetBytes((ushort)verseNumber), 0, 2);
                        }
                    }
                    cWordIndex.Add((int)(dataFile.Position / 2));
                }

                var cWordIndexJson = JsonConvert.SerializeObject(cWordIndex, Formatting.None);
                File.WriteAllText("concordanceIndex.js", "define(function () { return " + cWordIndexJson + "; });", utf8);
            }
        }

        public static void Stats(IReadOnlyList<Verse> verses)
        {
            Console.WriteLine("Books: " + verses.NumberOfBooks());
            Console.WriteLine("Verses: " + verses.Count);
            Console.WriteLine("Words: " + verses.NumberOfWords());
            Console.WriteLine("CIWords: " + verses.NumberOfCaseInsensitiveWords());
            Console.WriteLine("Punct: " + verses.NumberOfPunctuation());
            Console.WriteLine("Total text: " + verses.SelectMany(x => x.Words).Count());
        }

        private static string CSharpSerialize(object obj)
        {
            if (obj is string || obj is int)
            {
                return JsonConvert.SerializeObject(obj);
            }
            if (obj is IEnumerable enumerable)
            {
                return "{\n" + string.Join(",\n", enumerable.Cast<object>().Select(CSharpSerialize)) + "\n}";
            }
            if (obj is Structure structure)
            {
                return $"new Structure(new Book[] {CSharpSerialize(structure.Books)})";
            }
            if (obj is Book book)
            {
                return $"new Book({CSharpSerialize(book.Name)}, {book.BeginVerse}, {book.EndVerse}, new Chapter[] {CSharpSerialize(book.Chapters)})";
            }
            if (obj is Chapter chapter)
            {
                return $"new Chapter({chapter.BeginVerse}, {chapter.EndVerse})";
            }
            throw new InvalidOperationException($"Can't serialize type {obj.GetType().Name} to C#");
        }
    }
}
