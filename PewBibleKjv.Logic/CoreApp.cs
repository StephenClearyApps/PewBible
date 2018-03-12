using System;
using System.Collections.Generic;
using System.Text;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;

namespace PewBibleKjv.Logic
{
    public sealed class CoreApp : IDisposable
    {
        private readonly IChapterHeading _chapterHeading;
        private readonly IVerseView _verseView;
        private readonly IHistoryControls _historyControls;
        private readonly History _history;

        public CoreApp(IChapterHeading chapterHeading, IVerseView verseView, ISimpleStorage simpleStorage,
            IHistoryControls historyControls, int initialJump)
        {
            _chapterHeading = chapterHeading;
            _verseView = verseView;
            _historyControls = historyControls;

            // Load history.
            _history = new History(simpleStorage);

            // Keep track of changes to the verse view.
            verseView.OnScroll += UpdateCurrentLocation;
            verseView.OnSwipeLeft += MoveNextChapter;
            verseView.OnSwipeRight += MovePreviousChapter;

            // Wire up history to the history controls.
            historyControls.BackClick += MoveBack;
            historyControls.ForwardClick += MoveForward;
            _history.CanMoveChanged += EnableDisableHistoryButtons;

            // If the app has to jump to a verse, then insert it into the history.
            if (initialJump != Bible.InvalidAbsoluteVerseNumber)
                _history.AddJump(_history.CurrentAbsoluteVerseNumber, initialJump);

            // Pick up where we left off.
            verseView.Jump(Location.Create(_history.CurrentAbsoluteVerseNumber));

            EnableDisableHistoryButtons();
        }

        public void Dispose()
        {
            _history.Save(_verseView.CurrentAbsoluteVerseNumber);
            _verseView.OnScroll -= UpdateCurrentLocation;
            _verseView.OnSwipeLeft -= MoveNextChapter;
            _verseView.OnSwipeRight -= MovePreviousChapter;
            _historyControls.BackClick -= MoveBack;
            _historyControls.ForwardClick -= MoveForward;
            _history.CanMoveChanged -= EnableDisableHistoryButtons;
        }

        private void EnableDisableHistoryButtons()
        {
            _historyControls.BackEnabled = _history.CanMoveBack;
            _historyControls.ForwardEnabled = _history.CanMoveForward;
        }

        private void MoveForward()
        {
            _verseView.Jump(Location.Create(_history.MoveForward(_verseView.CurrentAbsoluteVerseNumber)));
        }

        private void MoveBack()
        {
            _verseView.Jump(Location.Create(_history.MoveBack(_verseView.CurrentAbsoluteVerseNumber)));
        }

        private void MovePreviousChapter(Location startSwipeLocation)
        {
            _verseView.Jump(startSwipeLocation.PreviousChapter());
        }

        private void MoveNextChapter(Location startSwipeLocation)
        {
            _verseView.Jump(startSwipeLocation.NextChapter());
        }

        private void UpdateCurrentLocation()
        {
            _chapterHeading.Text = _verseView.CurrentVerseLocation.ChapterHeadingText;
        }
    }
}
