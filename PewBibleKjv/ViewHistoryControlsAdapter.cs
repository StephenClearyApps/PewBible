using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Logic.Adapters.UI;

namespace PewBibleKjv
{
    public sealed class ViewHistoryControlsAdapter: IHistoryControls
    {
        private readonly View _backButton;
        private readonly View _forwardButton;

        public ViewHistoryControlsAdapter(View backButton, View forwardButton)
        {
            _backButton = backButton;
            _forwardButton = forwardButton;

            _backButton.Click += (_, __) => BackClick?.Invoke();
            _forwardButton.Click += (_, __) => ForwardClick?.Invoke();
        }

        public event Action BackClick;
        public event Action ForwardClick;
        public bool BackEnabled
        {
            set
            {
                _backButton.Enabled = value;
                _backButton.Alpha = value ? 1.0f : 0.5f;
            }
        }

        public bool ForwardEnabled
        {
            set
            {
                _forwardButton.Enabled = value;
                _forwardButton.Alpha = value ? 1.0f : 0.5f;
            }
        }
    }
}