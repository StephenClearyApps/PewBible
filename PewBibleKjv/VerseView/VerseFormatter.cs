using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using PewBibleKjv.Text;
using JavaObject = Java.Lang.Object;

namespace PewBibleKjv.VerseView;

public static class VerseFormatter
{
    private static readonly SimpleCache<StyleSpan> BoldStyleSpanCache = new SimpleCache<StyleSpan>(() => new StyleSpan(TypefaceStyle.Bold));
    private static readonly SimpleCache<TypefaceSpan> SansSerifTypespaceSpanCache = new SimpleCache<TypefaceSpan>(() => new TypefaceSpan("sans-serif"));
    private static readonly SimpleCache<UnderlineSpan> UnderlineSpanCache = new SimpleCache<UnderlineSpan>(() => new UnderlineSpan());
    private static readonly SimpleCache<StyleSpan> ItalicsStyleSpanCache = new SimpleCache<StyleSpan>(() => new StyleSpan(TypefaceStyle.Italic));
    private static readonly SimpleCache<RelativeSizeSpan> SmallerRelativeSizeSpanCache = new SimpleCache<RelativeSizeSpan>(() => new RelativeSizeSpan(0.8f));

    public static void ApplyFormattedText(VerseViewHolder @this)
    {
        @this.ApplyChapterText();

        var prefix = @this.Location!.Verse + " ";
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
        string? chapterText = null;
        if (@this.Location!.Verse == 1)
            chapterText = @this.Location.ChapterHeadingText;

        @this.ChapterHeaderView.Visibility = chapterText == null ? ViewStates.Gone : ViewStates.Visible;
        @this.HorizontalLine.Visibility = chapterText == null ? ViewStates.Gone : ViewStates.Visible;
        if (chapterText != null)
            @this.ChapterHeaderView.Text = chapterText;
    }

    public static void Free(VerseViewHolder @this)
    {
        foreach (var span in @this.SpanObjects)
            span.Free();
        @this.SpanObjects.Clear();
    }

    private static JavaObject AddTo(this ISimpleCacheItem<JavaObject> @this, List<ISimpleCacheItem<JavaObject>> collection)
    {
        collection.Add(@this);
        return @this.Instance;
    }
}