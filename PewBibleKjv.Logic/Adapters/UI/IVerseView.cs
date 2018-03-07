using System;

namespace PewBibleKjv.Logic.Adapters.UI
{
    /// <summary>
    /// The UI that displays a scrollable list of verses.
    /// </summary>
    public interface IVerseView
    {
        /// <summary>
        /// Notification that the user (or <see cref="Jump"/>) has scrolled the view. <see cref="CurrentAbsoluteVerseNumber"/> should already be updated before this event is raised.
        /// </summary>
        event Action OnScroll;

        /// <summary>
        /// Notification that the user has swiped left. <see cref="CurrentAbsoluteVerseNumber"/> has not been updated yet.
        /// The parameter is the verse location that was current at the time the swipe was started.
        /// </summary>
        event Action<Location> OnSwipeLeft;

        /// <summary>
        /// Notification that the user has swiped left. <see cref="CurrentAbsoluteVerseNumber"/> has not been updated yet.
        /// The parameter is the verse location that was current at the time the swipe was started.
        /// </summary>
        event Action<Location> OnSwipeRight;

        /// <summary>
        /// Returns the current verse number at the top of the view.
        /// </summary>
        int CurrentAbsoluteVerseNumber { get; }

        /// <summary>
        /// Returns the current verse location at the top of the view. This is equivalent to but more efficient than <c>Location.Create(CurrentAbsoluteVerseNumber)</c>.
        /// </summary>
        Location CurrentVerseLocation { get; }

        /// <summary>
        /// Skips the view to the specified verse number. Eventually raises <see cref="OnScroll"/> (not necessarily synchronously).
        /// </summary>
        /// <param name="absoluteVerseNumber">The verse number to scroll to.</param>
        void Jump(int absoluteVerseNumber);
    }
}
