using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Text;
using Object = Java.Lang.Object;

namespace PewBibleKjv
{
    [Activity(Label = "Choose Chapter")]
    public class ChooseChapterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var bookIndex = Intent.GetIntExtra("BookIndex", -1);
            if (bookIndex == -1)
            {
                StartActivity(typeof(MainActivity));
                return;
            }

            // Set our view from our layout resource
            SetContentView(Resource.Layout.ChooseChapter);

            var heading = FindViewById<TextView>(Resource.Id.chooseChapterHeading);
            heading.Text = Structure.Books[bookIndex].Name;

            var grid = FindViewById<GridView>(Resource.Id.chooseChapterGrid);
            grid.SetColumnWidth(MaximumButtonWidth());
            grid.Adapter = new ChapterAdapter(this, bookIndex);
        }

        private int MaximumButtonWidth()
        {
            var buffer = new FrameLayout(this);
            var view = LayoutInflater.Inflate(Resource.Layout.ChooseChapterButton, buffer, false);
            var button = view.FindViewById<Button>(Resource.Id.chooseChapterButton);
            button.Text = (Structure.Books.Max(x => x.Chapters.Length) + 1).ToString();
            buffer.AddView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));

            view.ForceLayout();
            view.Measure(1000, 1000);
            var width = view.MeasuredWidth;

            buffer.RemoveAllViews();
            return width;
        }

        private sealed class ChapterAdapter : ArrayAdapter<int>
        {
            private readonly Context _context;
            private readonly int _bookIndex;

            public ChapterAdapter(Context context, int bookIndex)
                : base(context, Resource.Layout.ChooseChapterButton, Structure.Books[bookIndex].Chapters.Select((_, i) => i + 1).ToArray())
            {
                _context = context;
                _bookIndex = bookIndex;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var result = base.GetView(position, convertView, parent);
                var button = result.FindViewById<Button>(Resource.Id.chooseChapterButton);
                button.Click += (_, __) =>
                {
                    var chapterIndex = position;
                    var activity = new Intent(_context, typeof(MainActivity));
                    activity.PutExtra("BookIndex", _bookIndex);
                    activity.PutExtra("ChapterIndex", chapterIndex);
                    _context.StartActivity(activity);
                };
                return result;
            }
        }
    }
}