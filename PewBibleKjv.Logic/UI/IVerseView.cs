using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Logic.UI
{
    public interface IVerseView
    {
        event Action<Location> OnScroll;
        event Action<Location> OnJump;
        void Jump(int absoluteVerseNumber);
    }
}
