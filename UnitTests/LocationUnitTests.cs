using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using Xunit;

namespace UnitTests
{
    public class LocationUnitTests
    {
        [Fact]
        public void Verse_26045_is_John_1_1()
        {
            var location = Location.Create(26045);
            Assert.Equal("John", location.Book.Name);
            Assert.Equal(1, location.ChapterNumber);
            Assert.Equal(1, location.Verse);
        }

        [Fact]
        public void Verse_0_is_Gen_1_1()
        {
            var location = Location.Create(0);
            Assert.Equal("Genesis", location.Book.Name);
            Assert.Equal(1, location.ChapterNumber);
            Assert.Equal(1, location.Verse);
        }

        [Fact]
        public void Verse_31101_is_Rev_22_21()
        {
            var location = Location.Create(31101);
            Assert.Equal("Revelation", location.Book.Name);
            Assert.Equal(22, location.ChapterNumber);
            Assert.Equal(21, location.Verse);
        }
    }
}
