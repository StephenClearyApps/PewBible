using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Logic;

namespace PewBibleKjv.VerseView
{
    public class VerseViewHolder : RecyclerView.ViewHolder
    {
        public TextView View { get; }
        public Location Location { get; set; }

        public VerseViewHolder(View view) : base(view)
        {
            View = view.FindViewById<TextView>(Resource.Id.verseText);
        }
    }
}

