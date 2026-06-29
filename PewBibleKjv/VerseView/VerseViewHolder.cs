using Android.Views;
using AndroidX.RecyclerView.Widget;
using PewBibleKjv.Logic;
using JavaObject = Java.Lang.Object;

namespace PewBibleKjv.VerseView;

public class VerseViewHolder : RecyclerView.ViewHolder
{
    public TextView ChapterHeaderView { get; }
    public View HorizontalLine { get; }
    public TextView View { get; }
    public Location? Location { get; private set; }
    public List<ISimpleCacheItem<JavaObject>> SpanObjects { get; } = [];

    public VerseViewHolder(View view) : base(view)
    {
        ChapterHeaderView = view.FindViewById<TextView>(Resource.Id.verseChapterHeaderText)!;
        HorizontalLine = view.FindViewById<View>(Resource.Id.verseHorizontalLine)!;
        View = view.FindViewById<TextView>(Resource.Id.verseText)!;
    }

    public void Bind(int position)
    {
        Location = Location.Create(position);
        if (SpanObjects.Count != 0)
            VerseFormatter.Free(this);
        VerseFormatter.ApplyFormattedText(this);
    }

    public void Unbind()
    {
        VerseFormatter.Free(this);
        Location = null;
    }
}

