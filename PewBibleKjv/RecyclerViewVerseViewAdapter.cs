using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.UI;
using PewBibleKjv.Text;
using PewBibleKjv.Util;
using PewBibleKjv.VerseView;

namespace PewBibleKjv
{
    public sealed class RecyclerViewVerseViewAdapter: IVerseView
    {
        private readonly RecyclerView _recyclerView;
        private readonly LinearLayoutManager _layoutManager;
        private readonly RecyclerViewScrollListener _scrollListener;
        private readonly SwipeTouchListener _swipeTouchListener;
        private int _lastPosition = Bible.InvalidAbsoluteVerseNumber;
        private Location _startSwipeLocation;

        public RecyclerViewVerseViewAdapter(Context context, RecyclerView recyclerView, LinearLayoutManager layoutManager)
        {
            _recyclerView = recyclerView;
            _layoutManager = layoutManager;

            _scrollListener = new RecyclerViewScrollListener();
            _scrollListener.Scrolled += ScrollListenerOnScrolled;
            _recyclerView.AddOnScrollListener(_scrollListener);

            _swipeTouchListener = new SwipeTouchListener(context);
            _swipeTouchListener.OnDown += SwipeTouchListenerOnOnDown;
            _swipeTouchListener.OnSwipeLeft += SwipeTouchListenerOnOnSwipeLeft;
            _swipeTouchListener.OnSwipeRight += SwipeTouchListenerOnOnSwipeRight;
            _recyclerView.SetOnTouchListener(_swipeTouchListener);
        }

        public event Action OnScroll;
        public event Action<Location> OnSwipeLeft;
        public event Action<Location> OnSwipeRight;

        public int CurrentAbsoluteVerseNumber => _layoutManager.FindFirstVisibleItemPosition();

        public Location CurrentVerseLocation
        {
            get
            {
                var viewHolder = _recyclerView.FindViewHolderForLayoutPosition(CurrentAbsoluteVerseNumber);
                return ((VerseViewHolder)viewHolder).Location;
            }
        }

        public void Jump(int absoluteVerseNumber) => _layoutManager.ScrollToPositionWithOffset(absoluteVerseNumber, 0);

        private void SwipeTouchListenerOnOnDown() => _startSwipeLocation = CurrentVerseLocation;

        private void SwipeTouchListenerOnOnSwipeRight() => OnSwipeRight?.Invoke(_startSwipeLocation);

        private void SwipeTouchListenerOnOnSwipeLeft() => OnSwipeLeft?.Invoke(_startSwipeLocation);

        private void ScrollListenerOnScrolled(RecyclerView recyclerView, int i, int arg3)
        {
            var firstIndex = CurrentAbsoluteVerseNumber;
            if (firstIndex == _lastPosition)
                return;
            _lastPosition = firstIndex;
            OnScroll?.Invoke();
        }
    }
}