using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic.Adapters.UI;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubHistoryControls: IHistoryControls
    {
        public event Action BackClick;
        public void RaiseBackClick() => BackClick?.Invoke();
        public event Action ForwardClick;
        public void RaiseForwardClick() => ForwardClick?.Invoke();
        public bool BackEnabled { get; set; }
        public bool ForwardEnabled { get; set; }
    }
}
