using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.Comparers;
using PewBibleKjv.Text;

namespace UnitTests
{
    public static class Utility
    {
        public static IFullComparer<FormattedVerse.Span> SpanComparer = ComparerBuilder.For<FormattedVerse.Span>()
            .OrderBy(x => x.Type).ThenBy(x => x.Begin).ThenBy(x => x.End);
    }
}
