using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using Object = Java.Lang.Object;

namespace PewBibleKjv.VerseView
{
    public static class VerseFormatter
    {
        private static readonly SimpleCache<StyleSpan> BoldStyleSpanCache = new SimpleCache<StyleSpan>(() => new StyleSpan(TypefaceStyle.Bold));
        private static readonly SimpleCache<TypefaceSpan> SansSerifTypespaceSpanCache = new SimpleCache<TypefaceSpan>(() => new TypefaceSpan("sans-serif"));
        private static readonly SimpleCache<UnderlineSpan> UnderlineSpanCache = new SimpleCache<UnderlineSpan>(() => new UnderlineSpan());
        private static readonly SimpleCache<AlignmentSpanStandard> AlignCenterSpanCache = new SimpleCache<AlignmentSpanStandard>(() => new AlignmentSpanStandard(Layout.Alignment.AlignCenter));
        private static readonly SimpleCache<StyleSpan> ItalicsStyleSpanCache = new SimpleCache<StyleSpan>(() => new StyleSpan(TypefaceStyle.Italic));
        private static readonly SimpleCache<RelativeSizeSpan> SmallerRelativeSizeSpanCache = new SimpleCache<RelativeSizeSpan>(() => new RelativeSizeSpan(0.8f));

        public static SpannableString FormattedText(Location location, List<ISimpleCacheItem<Object>> spans)
        {
            var formattedVerse = Bible.FormattedVerse(location.AbsoluteVerseNumber);
            var chapterPrefix = "";
            if (location.Verse == 1)
                chapterPrefix = "\n" + location.ChapterHeadingText + "\n\n";
            var prefix = chapterPrefix + location.Verse + " ";
            var formattedText = new SpannableString(prefix + formattedVerse.Text);
            formattedText.SetSpan(BoldStyleSpanCache.Alloc().AddTo(spans), 0, prefix.Length, SpanTypes.InclusiveExclusive);
            formattedText.SetSpan(SansSerifTypespaceSpanCache.Alloc().AddTo(spans), 0, prefix.Length, SpanTypes.InclusiveExclusive);
            if (chapterPrefix != "")
            {
                formattedText.SetSpan(UnderlineSpanCache.Alloc().AddTo(spans), 0, chapterPrefix.Length, SpanTypes.InclusiveExclusive);
                formattedText.SetSpan(AlignCenterSpanCache.Alloc().AddTo(spans), 0, chapterPrefix.Length, SpanTypes.InclusiveExclusive);
            }

            foreach (var textSpan in formattedVerse.Spans)
            {
                var span =
                    textSpan.Type == FormattedVerse.SpanType.Italics ? ItalicsStyleSpanCache.Alloc().AddTo(spans) :
                    textSpan.Type == FormattedVerse.SpanType.Colophon ? SmallerRelativeSizeSpanCache.Alloc().AddTo(spans) :
                    throw new InvalidOperationException($"Unknown span type {textSpan.Type}");
                formattedText.SetSpan(span, textSpan.Begin + prefix.Length,
                    textSpan.End + prefix.Length, SpanTypes.InclusiveExclusive);
            }

            return formattedText;
        }

        public static void Free(List<ISimpleCacheItem<Object>> spans)
        {
            foreach (var span in spans)
                span.Free();
            spans.Clear();
        }

        private static Object AddTo(this ISimpleCacheItem<Object> @this, List<ISimpleCacheItem<Object>> collection)
        {
            collection.Add(@this);
            return @this.Instance;
        }
    }
}