using System.Diagnostics.CodeAnalysis;
using Nito.Comparers;
using PewBibleKjv.Text;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public static class Comparers
    {
        public static IFullComparer<FormattedVerse.Span> SpanComparer = ComparerBuilder.For<FormattedVerse.Span>()
            .OrderBy(x => x.Type).ThenBy(x => x.Begin).ThenBy(x => x.End);
    }
}
