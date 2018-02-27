using System;
using System.Collections.Generic;
using System.Text;
using PewBibleKjv.Logic.UI;

namespace PewBibleKjv.Logic
{
    public sealed class App
    {
        public App(IChapterHeading chapterHeading, IVerseView verseView)
        {
            // Whenever the verse view is scrolled or jumped to a new location, update the chapter heading.
            verseView.OnJump += location => chapterHeading.Text = location.ChapterHeadingText;
            verseView.OnScroll += location => chapterHeading.Text = location.ChapterHeadingText;

            // Start at John 1:1
            verseView.Jump(26045);
        }
    }
}
