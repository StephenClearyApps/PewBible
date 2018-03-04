using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;

namespace UnitTests.Util
{
    public static class AppFactory
    {
        public static App Create(IChapterHeading chapterHeading = null, IVerseView verseView = null,
            ISimpleStorage simpleStorage = null, IHistoryControls historyControls = null,
            int initialJump = Bible.InvalidAbsoluteVerseNumber)
        {
            return new App(chapterHeading ?? new StubChapterHeading(), verseView ?? new StubVerseView(),
                simpleStorage ?? new StubSimpleStorage(), historyControls ?? new StubHistoryControls(),
                initialJump);
        }
    }
}
