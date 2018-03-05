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
    public class AppHistoryUnitTests
    {
        [Fact]
        public void InitialStartup_HistoryButtonsDisabled()
        {
            var app = new StubbedApp();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
        }

        [Fact]
        public void History_TwoItems()
        {
            var app = new StubbedApp();
            var verse1 = VerseHelper.Find("Luke", 3, 13).AbsoluteVerseNumber;
            app.Recreate(verse1);
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseBackClick();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void History_ThreeItems()
        {
            var app = new StubbedApp();
            var verse1 = VerseHelper.Find("Luke", 3, 13).AbsoluteVerseNumber;
            var verse2 = VerseHelper.Find("Psalms", 23).AbsoluteVerseNumber;
            app.Recreate(verse1);
            app.Recreate(verse2);
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseBackClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseBackClick();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void History_ThreeItems_JumpInMiddle()
        {
            var app = new StubbedApp();
            var verse1 = VerseHelper.Find("Luke", 3, 13).AbsoluteVerseNumber;
            var verse2 = VerseHelper.Find("Psalms", 23).AbsoluteVerseNumber;
            app.Recreate(verse1);
            app.StubHistoryControls.RaiseBackClick();
            app.Recreate(verse2);
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseBackClick();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse2, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseForwardClick();
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void History_JumpToNextLocation_DoesNotAddDuplicate()
        {
            var app = new StubbedApp();
            var verse1 = VerseHelper.Find("Luke", 3, 13).AbsoluteVerseNumber;
            app.Recreate(verse1);
            app.StubHistoryControls.RaiseBackClick();
            app.Recreate(verse1);

            Assert.False(app.StubHistoryControls.ForwardEnabled);
            Assert.Equal(verse1, app.StubVerseView.CurrentAbsoluteVerseNumber);

            app.StubHistoryControls.RaiseBackClick();
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.Equal(Bible.John_1_1, app.StubVerseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void History_JumpToSameLocation_DoesNotAddDuplicate()
        {
            var app = new StubbedApp();
            app.Recreate(Bible.John_1_1);
            
            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);
        }
    }
}
