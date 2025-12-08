using System.Reflection;

namespace PewBibleKjv.Text;

public static class Data
{
    private static readonly Lazy<Stream> _verses = new(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("PewBibleKjv.Text.Embedded.verses.dat")!);
    private static readonly Lazy<Stream> _verseIndex = new(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("PewBibleKjv.Text.Embedded.verseIndex.dat")!);

    public static Stream Verses => _verses.Value;
    public static Stream VerseIndex => _verseIndex.Value;
}
