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
using Xunit;

namespace UnitTests
{
    public class AppUnitTests
    {
        [Fact]
        public void InitialStartup_StartsAtJohn_1_1()
        {
            var verseView = new Mock<IVerseView>();
            var app = new App(Mock.Of<IChapterHeading>(), verseView.Object,
                Mock.Of<ISimpleStorage>(), Mock.Of<IHistoryControls>(), Bible.InvalidAbsoluteVerseNumber);
            verseView.Verify(x => x.Jump(26045));
        }

        [Fact]
        public void InitialStartup_HistoryButtonsDisabled()
        {
            var historyControls = new Mock<IHistoryControls>();
            var app = new App(Mock.Of<IChapterHeading>(), Mock.Of<IVerseView>(),
                Mock.Of<ISimpleStorage>(), historyControls.Object, Bible.InvalidAbsoluteVerseNumber);
            historyControls.VerifySet(x => x.BackEnabled = false);
            historyControls.VerifySet(x => x.ForwardEnabled = false);
        }
    }
}
