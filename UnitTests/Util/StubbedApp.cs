using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubbedApp
    {
        public StubbedApp(IChapterHeading chapterHeading = null, IVerseView verseView = null,
            ISimpleStorage simpleStorage = null, IHistoryControls historyControls = null,
            int initialJump = Bible.InvalidAbsoluteVerseNumber)
        {
            ChapterHeading = chapterHeading ?? new StubChapterHeading();
            VerseView = verseView ?? new StubVerseView();
            SimpleStorage = simpleStorage ?? new StubSimpleStorage();
            HistoryControls = historyControls ?? new StubHistoryControls();
            InitialJump = initialJump;
            App = new CoreApp(ChapterHeading, VerseView, SimpleStorage, HistoryControls, InitialJump);
        }

        public IChapterHeading ChapterHeading { get; }
        public StubChapterHeading StubChapterHeading => ChapterHeading as StubChapterHeading;
        public IVerseView VerseView { get; }
        public StubVerseView StubVerseView => VerseView as StubVerseView;
        public ISimpleStorage SimpleStorage { get; }
        public StubSimpleStorage StubSimpleStorage => SimpleStorage as StubSimpleStorage;
        public IHistoryControls HistoryControls { get; }
        public StubHistoryControls StubHistoryControls => HistoryControls as StubHistoryControls;
        public int InitialJump { get; private set; }

        public CoreApp App { get; private set; }

        public void Recreate(int initialJump = Bible.InvalidAbsoluteVerseNumber)
        {
            App.Dispose();
            InitialJump = initialJump;
            App = new CoreApp(ChapterHeading, VerseView, SimpleStorage, HistoryControls, InitialJump);
        }
    }
}
