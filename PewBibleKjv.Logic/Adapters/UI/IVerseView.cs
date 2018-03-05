using System;

namespace PewBibleKjv.Logic.Adapters.UI
{
    public interface IVerseView
    {
        event Action OnScroll;
        int CurrentAbsoluteVerseNumber { get; }
        void Jump(int absoluteVerseNumber);
        Location FindLocation(int absoluteVerseNumber);
    }
}
