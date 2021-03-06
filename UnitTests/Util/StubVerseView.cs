﻿using System;
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
        public event Action<Location> OnSwipeLeft;
        public event Action<Location> OnSwipeRight;

        public void RaiseOnScroll(int absoluteVerseNumber)
        {
            CurrentAbsoluteVerseNumber = absoluteVerseNumber;
            OnScroll?.Invoke();
        }

        public void RaiseOnSwipeLeft(Location startSwipeLocation = null) => OnSwipeLeft?.Invoke(startSwipeLocation ?? CurrentVerseLocation);
        public void RaiseOnSwipeRight(Location startSwipeLocation = null) => OnSwipeRight?.Invoke(startSwipeLocation ?? CurrentVerseLocation);

        public int CurrentAbsoluteVerseNumber { get; set; }
        public Location CurrentVerseLocation => Location.Create(CurrentAbsoluteVerseNumber);

        public void Jump(Location location) => RaiseOnScroll(location.AbsoluteVerseNumber);
    }
}
