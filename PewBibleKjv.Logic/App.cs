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
        private readonly IVerseView _verseView;
        private readonly History _history;

        public App(IChapterHeading chapterHeading, IVerseView verseView, ISimpleStorage simpleStorage, int initialJump)
        {
            _chapterHeading = chapterHeading;
            _verseView = verseView;

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

        public bool CanMovePrevious => _history.CanMovePrevious;
        public bool CanMoveNext => _history.CanMoveNext;
        public event Action CanMoveChanged
        {
            add => _history.CanMoveChanged += value;
            remove => _history.CanMoveChanged -= value;
        }
        
        public void Pause()
        {
            _history.Save(_verseView.CurrentAbsoluteVerseNumber);
        }

        public void MoveForward()
        {
            _verseView.Jump(_history.MoveNext(_verseView.CurrentAbsoluteVerseNumber));
        }

        public void MoveBack()
        {
            _verseView.Jump(_history.MovePrevious(_verseView.CurrentAbsoluteVerseNumber));
        }

        private void UpdateCurrentLocation(Location currentLocation)
        {
            _chapterHeading.Text = currentLocation.ChapterHeadingText;
        }
    }
}
