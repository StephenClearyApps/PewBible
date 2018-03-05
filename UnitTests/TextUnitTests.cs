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
        public void MostSpans()
        {
            // This verse has the most spans.
            var location = VerseHelper.Find("2 Corinthians", 11, 26);
            var verse = Bible.FormattedVerse(location.AbsoluteVerseNumber);
            Assert.Equal(" In journeyings often, in perils of waters, in perils of robbers, in perils by mine own countrymen, in perils by the heathen, in perils in the city, in perils in the wilderness, in perils in the sea, in perils among false brethren;", verse.Text);
            Assert.Equal(new []
            {
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 1,
                    End = 3,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 23,
                    End = 25,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 44,
                    End = 46,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 66,
                    End = 68,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 79,
                    End = 87,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 100,
                    End = 102,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 126,
                    End = 128,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 149,
                    End = 151,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 178,
                    End = 180,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 200,
                    End = 202,
                },
            }, verse.Spans, Comparers.SpanComparer);
        }

        [Fact]
        public void NestedSpans()
        {
            var location = VerseHelper.Find("Psalms", 11, 1);
            var verse = Bible.FormattedVerse(location.AbsoluteVerseNumber);
            Assert.Equal(" To the chief Musician, A Psalm of David. In the LORD put I my trust: how say ye to my soul, Flee as a bird to your mountain?", verse.Text);
            Assert.Equal(new[]
            {
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Colophon,
                    Begin = 1,
                    End = 41,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 24,
                    End = 31,
                },
                new FormattedVerse.Span
                {
                    Type = FormattedVerse.SpanType.Italics,
                    Begin = 98,
                    End = 100,
                },
            }, verse.Spans, Comparers.SpanComparer);
        }

        [Fact]
        public void Hyphen_IncludesZeroWidthSpace()
        {
            var location = VerseHelper.Find("Genesis", 33, 20);
            var verse = Bible.FormattedVerse(location.AbsoluteVerseNumber);
            Assert.Equal(" And he erected there an altar, and called it Elelohe-\u200BIsrael.", verse.Text);
        }

        [Fact]
        public void LongHyphen_DoesNotIncludeZeroWidthSpace()
        {
            // This is the only verse with a long hyphen.
            var location = VerseHelper.Find("Exodus", 32, 32);
            var verse = Bible.FormattedVerse(location.AbsoluteVerseNumber);
            Assert.Equal(" Yet now, if thou wilt forgive their sin—; and if not, blot me, I pray thee, out of thy book which thou hast written.", verse.Text);
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
