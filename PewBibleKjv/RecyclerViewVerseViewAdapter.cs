using Android.Content;
using AndroidX.RecyclerView.Widget;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;
using PewBibleKjv.Util;
using PewBibleKjv.VerseView;

namespace PewBibleKjv;

public sealed class RecyclerViewVerseViewAdapter: IVerseView
{
    private readonly int _chapterHeadingVerseOffset;
    private readonly RecyclerView _recyclerView;
    private readonly LinearLayoutManager _layoutManager;
    private readonly RecyclerViewScrollListener _scrollListener;
    private int _lastPosition = Bible.InvalidAbsoluteVerseNumber;

    public RecyclerViewVerseViewAdapter(Context context, RecyclerView recyclerView, LinearLayoutManager layoutManager, int chapterHeadingVerseOffset)
    {
        _recyclerView = recyclerView;
        _layoutManager = layoutManager;
        _chapterHeadingVerseOffset = chapterHeadingVerseOffset;

        _scrollListener = new RecyclerViewScrollListener();
        _scrollListener.Scrolled += ScrollListenerOnScrolled;
        _recyclerView.AddOnScrollListener(_scrollListener);
    }

    public event Action? OnScroll;
    public event Action<Location>? OnSwipeLeft;
    public event Action<Location>? OnSwipeRight;

    public int CurrentAbsoluteVerseNumber => _layoutManager.FindFirstVisibleItemPosition();

    public Location CurrentVerseLocation
    {
        get
        {
            var viewHolder = _recyclerView.FindViewHolderForLayoutPosition(CurrentAbsoluteVerseNumber)!;
            return ((VerseViewHolder)viewHolder).Location!;
        }
    }

    public void Jump(Location location) => _layoutManager.ScrollToPositionWithOffset(location.AbsoluteVerseNumber, location.Verse == 1 ? -_chapterHeadingVerseOffset : 0);

    private void ScrollListenerOnScrolled(RecyclerView recyclerView, int i, int arg3)
    {
        var firstIndex = CurrentAbsoluteVerseNumber;
        if (firstIndex == _lastPosition)
            return;
        _lastPosition = firstIndex;
        OnScroll?.Invoke();
    }
}