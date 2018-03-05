using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
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
            var verse = Bible.VerseWords(VerseHelper.Find("Revelation", 22, 21).AbsoluteVerseNumber).ToArray();
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
            var verse = Bible.FormattedVerse(VerseHelper.Find("Revelation", 22, 21).AbsoluteVerseNumber);
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

        [Fact]
        public void AllVerses_PassBasicChecks()
        {
            for (var i = 0; i != VerseHelper.Find("Revelation", 22).Chapter.EndVerse; ++i)
            {
                var verse = Bible.FormattedVerse(i);
                Assert.False(string.IsNullOrWhiteSpace(verse.Text));
                foreach (var span in verse.Spans)
                {
                    Assert.True(span.Begin >= 0, $"Invalid span begin for {i}");
                    Assert.True(span.End <= verse.Text.Length, $"Invalid span end for {i}");
                    Assert.True(span.Begin < span.End, $"Invalid span for {i}");
                    Assert.False(string.IsNullOrWhiteSpace(verse.Text.Substring(span.Begin, span.End - span.Begin)), $"Invalid span text for {i}");
                }
            }
        }
    }
}
