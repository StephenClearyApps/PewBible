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

namespace PewBibleKjv
{
    public sealed class RecyclerViewVerseViewAdapter: IVerseView
    {
        private readonly RecyclerView _recyclerView;
        private readonly LinearLayoutManager _layoutManager;
        private readonly Func<int, Location> _positionToLocation;
        private readonly ScrollListener _scrollListener;
        private int _lastPosition = Bible.InvalidAbsoluteVerseNumber;
        private int _jumpTarget = Bible.InvalidAbsoluteVerseNumber;

        public RecyclerViewVerseViewAdapter(RecyclerView recyclerView, LinearLayoutManager layoutManager,
            Func<int, Location> positionToLocation)
        {
            _recyclerView = recyclerView;
            _layoutManager = layoutManager;
            _positionToLocation = positionToLocation;

            _scrollListener = new ScrollListener();
            _scrollListener.Scrolled += (_, __, ___) =>
            {
                var firstIndex = _layoutManager.FindFirstVisibleItemPosition();
                if (firstIndex == _lastPosition)
                    return;
                _lastPosition = firstIndex;
                var location = _positionToLocation(firstIndex);
                if (firstIndex == _jumpTarget)
                {
                    _jumpTarget = Bible.InvalidAbsoluteVerseNumber;
                    OnJump?.Invoke(location);
                }
                else
                {
                    OnScroll?.Invoke(location);
                }
            };
            _recyclerView.AddOnScrollListener(_scrollListener);
        }

        public event Action<Location> OnScroll;
        public event Action<Location> OnJump;

        public int CurrentAbsoluteVerseNumber => _layoutManager.FindFirstVisibleItemPosition();

        public void Jump(int absoluteVerseNumber)
        {
            _jumpTarget = absoluteVerseNumber;
            _layoutManager.ScrollToPositionWithOffset(absoluteVerseNumber, 0);
        }

        public class ScrollListener : RecyclerView.OnScrollListener
        {
            public override void OnScrollStateChanged(RecyclerView recyclerView, int newState) =>
                ScrollStateChanged?.Invoke(recyclerView, newState);

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy) =>
                Scrolled?.Invoke(recyclerView, dx, dy);

            public event Action<RecyclerView, int> ScrollStateChanged;
            public event Action<RecyclerView, int, int> Scrolled;
        }
    }
}