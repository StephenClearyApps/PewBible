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
    public class AppUnitTests
    {
        [Fact]
        public void InitialStartup_StartsAtJohn_1_1()
        {
            var app = new StubbedApp();
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void Scrolling_UpdatesHistory()
        {
            var app = new StubbedApp();
            var verse1 = Location.Create(Bible.John_1_1 + 13).AbsoluteVerseNumber;
            var verse2 = VerseHelper.Find("Psalms", 23).AbsoluteVerseNumber;
            app.StubVerseView.RaiseOnScroll(verse1);
            app.Recreate(verse2);
            app.StubHistoryControls.RaiseBackClick();
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }
    }
}
