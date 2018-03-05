using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic.Adapters.UI;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubChapterHeading: IChapterHeading
    {
        public string Text { get; set; }
    }
}
