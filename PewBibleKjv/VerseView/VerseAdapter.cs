using System;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;

namespace PewBibleKjv.VerseView
{
    public class VerseAdapter : RecyclerView.Adapter
    {
        private readonly TextService _data;
        private readonly LayoutInflater _layoutInflater;

        public VerseAdapter(TextService data, LayoutInflater layoutInflater)
        {
            _data = data;
            _layoutInflater = layoutInflater;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = (VerseViewHolder)holder;
            vh.Location = _data[position];
            var formattedVerse = Bible.FormattedVerse(vh.Location.AbsoluteVerseNumber);
            var chapterPrefix = "";
            if (vh.Location.Verse == 1)
                chapterPrefix = "\n" + vh.Location.ChapterHeadingText + "\n\n";
            var prefix = chapterPrefix + vh.Location.Verse + " ";
            var formattedText = new SpannableString(prefix + formattedVerse.Text);
            formattedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, prefix.Length, SpanTypes.InclusiveExclusive);
            formattedText.SetSpan(new TypefaceSpan("sans-serif"), 0, prefix.Length, SpanTypes.InclusiveExclusive);
            if (chapterPrefix != "")
            {
                formattedText.SetSpan(new UnderlineSpan(), 0, chapterPrefix.Length, SpanTypes.InclusiveExclusive);
                formattedText.SetSpan(new AlignmentSpanStandard(Layout.Alignment.AlignCenter), 0, chapterPrefix.Length, SpanTypes.InclusiveExclusive);
            }

            foreach (var textSpan in formattedVerse.Spans)
            {
                var span =
                    textSpan.Type == FormattedVerse.SpanType.Italics ? new StyleSpan(TypefaceStyle.Italic) as Java.Lang.Object :
                    textSpan.Type == FormattedVerse.SpanType.Colophon ? new RelativeSizeSpan(0.8f) :
                    throw new InvalidOperationException($"Unknown span type {textSpan.Type}");
                formattedText.SetSpan(span, textSpan.Begin + prefix.Length,
                    textSpan.End + prefix.Length, SpanTypes.InclusiveExclusive);
            }

            vh.View.TextFormatted = formattedText;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var verseView = _layoutInflater.Inflate(Resource.Layout.VerseLayout, parent, attachToRoot: false);
            return new VerseViewHolder(verseView);
        }

        public override int ItemCount => _data.Count;
    }
}