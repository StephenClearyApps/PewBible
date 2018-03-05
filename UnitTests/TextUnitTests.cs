using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    public class TextUnitTests
    {
        [Fact]
        public void Gen1_1_Words()
        {
            var verse = Bible.VerseWords(0).ToArray();
            Assert.Equal(new[] { "In", "the", "beginning", "God", "created", "the", "heaven", "and", "the", "earth", "." }, verse);
        }

        [Fact]
        public void Rev22_21_Words()
        {
            var verse = Bible.VerseWords(31101).ToArray();
            Assert.Equal(new[] { "The", "grace", "of", "our", "Lord", "Jesus", "Christ", "[", "be", "]", "with", "you", "all", ".", "Amen", "." }, verse);
        }

        [Fact]
        public void Gen1_1_Formatted()
        {
            var verse = Bible.FormattedVerse(0);
            Assert.Equal(" In the beginning God created the heaven and the earth.", verse.Text);
            Assert.Empty(verse.Spans);
        }

        [Fact]
        public void Rev22_21_Formatted()
        {
            var verse = Bible.FormattedVerse(31101);
            Assert.Equal(" The grace of our Lord Jesus Christ be with you all. Amen.", verse.Text);
            Assert.Equal(new []
            {
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 36,
                    End = 38,
                }
            }, verse.Spans, Comparers.SpanComparer);
        }
    }
}
