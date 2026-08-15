using System.Collections.Generic;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
    public class ConcordanceUnitTests
    {
        [Fact]
        public void Search_UnknownWord_ReturnsEmpty()
        {
            var results = Concordance.Search("xyzzynonexistentword");
            Assert.Empty(results);
        }

        [Fact]
        public void Search_EmptyQuery_ReturnsEmpty()
        {
            var results = Concordance.Search("   ");
            Assert.Empty(results);
        }

        [Fact]
        public void Search_SingleWord_ReturnsVerses()
        {
            var results = Concordance.Search("love");
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Search_John316_ContainsWell_KnownVerse()
        {
            // John 3:16 - "For God so loved the world..."
            var john316 = VerseHelper.Find("John", 3, 16).AbsoluteVerseNumber;

            var results = Concordance.Search("God world loved");
            Assert.Contains(john316, results);
        }

        [Fact]
        public void Search_MultiWord_IsSubsetOfEachSingleWordSearch()
        {
            var godResults = new HashSet<int>(Concordance.Search("god"));
            var worldResults = new HashSet<int>(Concordance.Search("world"));
            var combined = Concordance.Search("god world");

            Assert.All(combined, v =>
            {
                Assert.Contains(v, godResults);
                Assert.Contains(v, worldResults);
            });
        }

        [Fact]
        public void Search_IsCaseInsensitive()
        {
            var lower = Concordance.Search("jesus");
            var upper = Concordance.Search("JESUS");
            var mixed = Concordance.Search("Jesus");
            Assert.Equal(lower, upper);
            Assert.Equal(lower, mixed);
        }

        [Fact]
        public void Search_ResultsAreInCanonicalOrder()
        {
            var results = Concordance.Search("love");
            for (var i = 1; i < results.Count; i++)
                Assert.True(results[i] > results[i - 1], $"Results not in order at index {i}");
        }

        [Fact]
        public void Search_DuplicateWords_SameAsSingleWord()
        {
            var single = Concordance.Search("grace");
            var duplicate = Concordance.Search("grace grace");
            Assert.Equal(single, duplicate);
        }

        [Fact]
        public void Search_Gen1_1_ContainsBeginning()
        {
            // Genesis 1:1 - "In the beginning God created the heaven and the earth."
            var gen11 = VerseHelper.Find("Genesis", 1, 1).AbsoluteVerseNumber;
            var results = Concordance.Search("beginning");
            Assert.Contains(gen11, results);
        }
    }
}
