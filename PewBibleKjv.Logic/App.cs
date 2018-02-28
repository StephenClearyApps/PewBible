using System;
using System.Collections.Generic;
using System.Text;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
{
    public sealed class App
    {
        private readonly IChapterHeading _chapterHeading;
        private readonly History _history;
        private Location _currentLocation;

        public App(IChapterHeading chapterHeading, IVerseView verseView, ISimpleStorage simpleStorage, int initialJump)
        {
            _chapterHeading = chapterHeading;

            // Keep track of changes to the verse view.
            verseView.OnJump += UpdateCurrentLocation;
            verseView.OnScroll += UpdateCurrentLocation;

            // Load history.
            _history = new History(simpleStorage);

            // If the app has to jump to a verse, then insert it into the history.
            if (initialJump != Bible.InvalidAbsoluteVerseNumber)
                _history.SaveJump(_history.CurrentAbsoluteVerseNumber, initialJump);

            // Pick up where we left off.
            verseView.Jump(_history.CurrentAbsoluteVerseNumber);
        }

        private void UpdateCurrentLocation(Location currentLocation)
        {
            _currentLocation = currentLocation;
            _chapterHeading.Text = currentLocation.ChapterHeadingText;
        }

        public void Pause()
        {
            _history.Save(_currentLocation.AbsoluteVerseNumber);
        }
    }
}
