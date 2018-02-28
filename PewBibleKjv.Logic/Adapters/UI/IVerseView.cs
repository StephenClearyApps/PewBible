using System;

namespace PewBibleKjv.Logic.Adapters.UI
{
    public interface IVerseView
    {
        event Action<Location> OnScroll;
        event Action<Location> OnJump;
        void Jump(int absoluteVerseNumber);
    }
}
