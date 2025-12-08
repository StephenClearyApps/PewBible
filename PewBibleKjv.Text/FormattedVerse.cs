namespace PewBibleKjv.Text;

public sealed class FormattedVerse
{
    public required string Text { init; get; }

    public required List<Span> Spans { init; get; }

    public enum SpanType
    {
        Colophon,
        Italics
    }

    public sealed class Span
    {
        public SpanType Type { get; set; }
        public int Begin { get; set; }
        public int End { get; set; }
    }
}
