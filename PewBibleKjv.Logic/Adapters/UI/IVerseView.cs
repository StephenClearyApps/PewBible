using System;

namespace PewBibleKjv.Logic.Adapters.UI
{
    public interface IVerseView
    {
        event Action<Location> OnScroll;
        int CurrentAbsoluteVerseNumber { get; }
        void Jump(int absoluteVerseNumber);
    }
}
