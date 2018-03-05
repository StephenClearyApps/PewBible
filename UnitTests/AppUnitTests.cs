using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Text;
using UnitTests.Util;
using Xunit;

namespace UnitTests
{
    public class AppUnitTests
    {
        [Fact]
        public void InitialStartup_StartsAtJohn_1_1()
        {
            var app = new StubbedApp();
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }
    }
}
