using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PewBible.Text
{
    public static class Data
    {
        private static readonly Lazy<Stream> _verses = new Lazy<Stream>(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("PewBible.Text.Embedded.verses.dat"));

        public static Stream Verses => _verses.Value;
    }
}
