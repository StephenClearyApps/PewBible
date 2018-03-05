using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.UI;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubVerseView: IVerseView
    {
        public event Action OnScroll;
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;

        public void RaiseOnScroll(int absoluteVerseNumber)
        {
            CurrentAbsoluteVerseNumber = absoluteVerseNumber;
            OnScroll?.Invoke();
        }

        public void RaiseOnSwipeLeft() => OnSwipeLeft?.Invoke();
        public void RaiseOnSwipeRight() => OnSwipeRight?.Invoke();

        public int CurrentAbsoluteVerseNumber { get; set; }
        public Location CurrentVerseLocation => Location.Create(CurrentAbsoluteVerseNumber);

        public void Jump(int absoluteVerseNumber) => RaiseOnScroll(absoluteVerseNumber);
    }
}
