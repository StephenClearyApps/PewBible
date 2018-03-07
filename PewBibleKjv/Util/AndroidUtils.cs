using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PewBibleKjv.Util
{
    /// <summary>
    /// Allows measuring Android views offscreen.
    /// </summary>
    public sealed class AndroidUtils
    {
        private const int MeasureSpecUnspecified = 0;
        public static Size MeasureView(Context context, Func<ViewGroup, View> create)
        {
            var buffer = new FrameLayout(context);
            var view = create(buffer);

            view.ForceLayout();
            view.Measure(MeasureSpecUnspecified, MeasureSpecUnspecified);
            var result = new Size(view.MeasuredWidth, view.MeasuredHeight);

            buffer.RemoveAllViews();
            return result;
        }

        public static Size MeasureLayout(Context context, LayoutInflater layoutInflater, int layoutId, ViewGroup.LayoutParams layoutParams = null, Action<View> setupView = null)
        {
            layoutParams = layoutParams ?? new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            return MeasureView(context, buffer =>
            {
                var view = layoutInflater.Inflate(layoutId, buffer, false);
                setupView?.Invoke(view);
                buffer.AddView(view, layoutParams);
                return view;
            });
        }
    }
}