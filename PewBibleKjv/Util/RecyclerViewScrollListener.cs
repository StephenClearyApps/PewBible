using System;
using AndroidX.RecyclerView.Widget;

namespace PewBibleKjv.Util
{
    public sealed class RecyclerViewScrollListener: RecyclerView.OnScrollListener
    {
        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState) =>
            ScrollStateChanged?.Invoke(recyclerView, newState);

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy) =>
            Scrolled?.Invoke(recyclerView, dx, dy);

        public event Action<RecyclerView, int> ScrollStateChanged;
        public event Action<RecyclerView, int, int> Scrolled;
    }
}
