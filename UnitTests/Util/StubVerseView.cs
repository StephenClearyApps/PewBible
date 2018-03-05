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

        public void RaiseOnScroll(int absoluteVerseNumber)
        {
            CurrentAbsoluteVerseNumber = absoluteVerseNumber;
            OnScroll?.Invoke();
        }

        public int CurrentAbsoluteVerseNumber { get; set; }

        public void Jump(int absoluteVerseNumber) => RaiseOnScroll(absoluteVerseNumber);
        public Location FindLocation(int absoluteVerseNumber) => Location.Create(absoluteVerseNumber);
    }
}
