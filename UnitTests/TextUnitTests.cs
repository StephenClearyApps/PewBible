using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBible.Text;
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
            var verse = Bible.VerseWords(31101).ToArray();
            Assert.Equal(new[] { "The", "grace", "of", "our", "Lord", "Jesus", "Christ", "[", "be", "]", "with", "you", "all", ".", "Amen", "." }, verse);
        }
    }
}
