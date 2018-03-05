using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
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
        public void HistoryButtons_TwoItems()
        {
            var app = new StubbedApp();
            app.Recreate(VerseHelper.Find("Luke", 3, 13).AbsoluteVerseNumber);
            Assert.True(app.StubHistoryControls.BackEnabled);
            Assert.False(app.StubHistoryControls.ForwardEnabled);

            app.StubHistoryControls.RaiseBackClick();

            Assert.False(app.StubHistoryControls.BackEnabled);
            Assert.True(app.StubHistoryControls.ForwardEnabled);
        }
    }
}
