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
        private readonly IHistoryControls _historyControls;
        private readonly History _history;

        public App(IChapterHeading chapterHeading, IVerseView verseView, ISimpleStorage simpleStorage,
            IHistoryControls historyControls, int initialJump)
        {
            _chapterHeading = chapterHeading;
            _verseView = verseView;
            _historyControls = historyControls;

            // Keep track of changes to the verse view.
            verseView.OnScroll += UpdateCurrentLocation;

            // Load history.
            _history = new History(simpleStorage);

            // Wire up history to the history controls.
            historyControls.BackClick += MoveBack;
            historyControls.ForwardClick += MoveForward;
            _history.CanMoveChanged += EnableDisableHistoryButtons;

            // If the app has to jump to a verse, then insert it into the history.
            if (initialJump != Bible.InvalidAbsoluteVerseNumber)
                _history.SaveJump(_history.CurrentAbsoluteVerseNumber, initialJump);

            // Pick up where we left off.
            verseView.Jump(_history.CurrentAbsoluteVerseNumber);

            EnableDisableHistoryButtons();
        }

        public void Pause()
        {
            _history.Save(_verseView.CurrentAbsoluteVerseNumber);
        }

        private void EnableDisableHistoryButtons()
        {
            _historyControls.BackEnabled = _history.CanMovePrevious;
            _historyControls.ForwardEnabled = _history.CanMoveNext;
        }

        private void MoveForward()
        {
            _verseView.Jump(_history.MoveNext(_verseView.CurrentAbsoluteVerseNumber));
        }

        private void MoveBack()
        {
            _verseView.Jump(_history.MovePrevious(_verseView.CurrentAbsoluteVerseNumber));
        }

        private void UpdateCurrentLocation(Location currentLocation)
        {
            _chapterHeading.Text = currentLocation.ChapterHeadingText;
        }
    }
}
