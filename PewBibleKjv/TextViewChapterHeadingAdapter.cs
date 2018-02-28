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
    public sealed class TextViewChapterHeadingAdapter: IChapterHeading
    {
        private readonly TextView _view;

        public TextViewChapterHeadingAdapter(TextView view)
        {
            _view = view;
        }

        public string Text
        {
            set
            {
                if (_view.Text != value)
                    _view.Text = value;
            }
        }
    }
}