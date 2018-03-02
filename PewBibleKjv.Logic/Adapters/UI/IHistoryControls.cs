using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Logic.Adapters.UI
{
    public interface IHistoryControls
    {
        event Action BackClick;
        event Action ForwardClick;
        bool BackEnabled { set; }
        bool ForwardEnabled { set; }
    }
}
