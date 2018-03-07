using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using PewBibleKjv.Logic;
using System.Collections.Generic;

namespace PewBibleKjv.VerseView
{
    public class VerseViewHolder : RecyclerView.ViewHolder
    {
        public TextView ChapterHeaderView { get; }
        public View HorizontalLine { get; }
        public TextView View { get; }
        public Location Location { get; set; }
        public List<ISimpleCacheItem<Object>> SpanObjects { get; } = new List<ISimpleCacheItem<Object>>();

        public VerseViewHolder(View view) : base(view)
        {
            ChapterHeaderView = view.FindViewById<TextView>(Resource.Id.verseChapterHeaderText);
            HorizontalLine = view.FindViewById<View>(Resource.Id.verseHorizontalLine);
            View = view.FindViewById<TextView>(Resource.Id.verseText);
        }
    }
}

