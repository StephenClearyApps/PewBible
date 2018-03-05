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
        public event Action<Location> OnScroll;

        public void RaiseOnScroll(Location location)
        {
            CurrentAbsoluteVerseNumber = location.AbsoluteVerseNumber;
            OnScroll?.Invoke(location);
        }

        public int CurrentAbsoluteVerseNumber { get; set; }

        public void Jump(int absoluteVerseNumber) => RaiseOnScroll(Location.Create(absoluteVerseNumber));
    }
}
