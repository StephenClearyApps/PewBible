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
    public class AppUnitTests
    {
        [Fact]
        public void InitialStartup_StartsAtJohn_1_1()
        {
            var verseView = new StubVerseView();
            var app = AppFactory.Create(verseView: verseView);
            Assert.Equal(26045, verseView.CurrentAbsoluteVerseNumber);
        }

        [Fact]
        public void InitialStartup_HistoryButtonsDisabled()
        {
            var historyControls = new StubHistoryControls();
            var app = AppFactory.Create(historyControls: historyControls);
            Assert.False(historyControls.BackEnabled);
            Assert.False(historyControls.ForwardEnabled);
        }
    }
}
