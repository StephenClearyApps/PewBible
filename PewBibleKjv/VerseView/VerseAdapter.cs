using System;
using System.Diagnostics;
using System.Linq;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using Object = Java.Lang.Object;

namespace PewBibleKjv.VerseView
{
    public class VerseAdapter : RecyclerView.Adapter
    {
        private readonly LayoutInflater _layoutInflater;

        public VerseAdapter(LayoutInflater layoutInflater)
        {
            _layoutInflater = layoutInflater;
        }

        public override void OnViewRecycled(Object holder)
        {
            ((VerseViewHolder)holder).Unbind();
            base.OnViewRecycled(holder);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) => ((VerseViewHolder)holder).Bind(position);

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var verseView = _layoutInflater.Inflate(Resource.Layout.VerseLayout, parent, attachToRoot: false);
            return new VerseViewHolder(verseView);
        }

        public override int ItemCount { get; } = Structure.Books.Last().Chapters.Last().EndVerse;
    }
}