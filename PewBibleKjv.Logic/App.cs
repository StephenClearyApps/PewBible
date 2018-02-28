using System;
using System.Collections.Generic;
using System.Text;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;

namespace PewBibleKjv.Logic
{
    public sealed class App
    {
        public App(IChapterHeading chapterHeading, IVerseView verseView, ISimpleStorage simpleStorage)
        {
            // Whenever the verse view is scrolled or jumped to a new location, update the chapter heading.
            verseView.OnJump += location => chapterHeading.Text = location.ChapterHeadingText;
            verseView.OnScroll += location => chapterHeading.Text = location.ChapterHeadingText;

            // Load history.
            History = new History(simpleStorage);

            // Pick up where we left off.
            verseView.Jump(History.CurrentAbsoluteVerseNumber);
        }

        public History History { get; }
    }
}
