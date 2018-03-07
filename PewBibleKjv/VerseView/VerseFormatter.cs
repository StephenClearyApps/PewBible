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
        private static readonly SimpleCache<StyleSpan> ItalicsStyleSpanCache = new SimpleCache<StyleSpan>(() => new StyleSpan(TypefaceStyle.Italic));
        private static readonly SimpleCache<RelativeSizeSpan> SmallerRelativeSizeSpanCache = new SimpleCache<RelativeSizeSpan>(() => new RelativeSizeSpan(0.8f));

        public static void ApplyFormattedText(this VerseViewHolder @this)
        {
            @this.ApplyChapterText();

            var prefix = @this.Location.Verse + " ";
            var formattedVerse = Bible.FormattedVerse(@this.Location.AbsoluteVerseNumber);
            var formattedText = new SpannableString(prefix + formattedVerse.Text);
            formattedText.SetSpan(BoldStyleSpanCache.Alloc().AddTo(@this.SpanObjects), 0, prefix.Length, SpanTypes.InclusiveExclusive);
            formattedText.SetSpan(SansSerifTypespaceSpanCache.Alloc().AddTo(@this.SpanObjects), 0, prefix.Length, SpanTypes.InclusiveExclusive);

            foreach (var textSpan in formattedVerse.Spans)
            {
                var span =
                    textSpan.Type == FormattedVerse.SpanType.Italics ? ItalicsStyleSpanCache.Alloc().AddTo(@this.SpanObjects) :
                    textSpan.Type == FormattedVerse.SpanType.Colophon ? SmallerRelativeSizeSpanCache.Alloc().AddTo(@this.SpanObjects) :
                    throw new InvalidOperationException($"Unknown span type {textSpan.Type}");
                formattedText.SetSpan(span, textSpan.Begin + prefix.Length, textSpan.End + prefix.Length, SpanTypes.InclusiveExclusive);
            }

            @this.View.TextFormatted = formattedText;
        }

        private static void ApplyChapterText(this VerseViewHolder @this)
        {
            SpannableString chapterText = null;
            if (@this.Location.Verse == 1)
            {
                var text = @this.Location.ChapterHeadingText;
                chapterText = new SpannableString(text);
                chapterText.SetSpan(UnderlineSpanCache.Alloc().AddTo(@this.SpanObjects), 0, text.Length, SpanTypes.InclusiveExclusive);
            }

            @this.ChapterHeaderView.Visibility = chapterText == null ? ViewStates.Gone : ViewStates.Visible;
            if (chapterText != null)
                @this.ChapterHeaderView.TextFormatted = chapterText;
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