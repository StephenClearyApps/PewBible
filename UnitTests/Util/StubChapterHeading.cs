using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic.Adapters.UI;

namespace UnitTests.Util
{
    public sealed class StubChapterHeading: IChapterHeading
    {
        public string Text { get; set; }
    }
}
