using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Logic.Adapters.UI
{
    /// <summary>
    /// The UI that allows the user to move through history.
    /// </summary>
    public interface IHistoryControls
    {
        /// <summary>
        /// The user clicked the back button. This event should only be raised if <see cref="BackEnabled"/> is <c>true</c>.
        /// </summary>
        event Action BackClick;

        /// <summary>
        /// The user clicked the forward button. This event should only be raised if <see cref="ForwardEnabled"/> is <c>true</c>.
        /// </summary>
        event Action ForwardClick;

        /// <summary>
        /// Whether the back button is enabled.
        /// </summary>
        bool BackEnabled { set; }

        /// <summary>
        /// Whether the forward button is enabled.
        /// </summary>
        bool ForwardEnabled { set; }
    }
}
