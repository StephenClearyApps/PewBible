using System.Text;

namespace PewBibleKjv.Text;

public static class Concordance
{
    private static readonly Lazy<int[][]> _index = new(BuildIndex, LazyThreadSafetyMode.ExecutionAndPublication);

    private static int[][] BuildIndex()
    {
        var totalVerses = Structure.Books[Structure.Books.Length - 1].EndVerse;
        var lists = new List<int>[Constants.Words.Length];
        for (var i = 0; i < lists.Length; i++)
            lists[i] = new List<int>();

        var seenInVerse = new HashSet<int>();
        for (var verseNumber = 0; verseNumber < totalVerses; verseNumber++)
        {
            seenInVerse.Clear();
            foreach (var word in Bible.VerseWords(verseNumber))
            {
                if (word.Length == 0 || !char.IsLetter(word[0]))
                    continue;
                var wordIndex = Array.BinarySearch(Constants.Words, word.ToLowerInvariant(), StringComparer.Ordinal);
                if (wordIndex >= 0 && seenInVerse.Add(wordIndex))
                    lists[wordIndex].Add(verseNumber);
            }
        }

        var result = new int[lists.Length][];
        for (var i = 0; i < lists.Length; i++)
            result[i] = lists[i].ToArray();
        return result;
    }

    /// <summary>
    /// Searches for verses containing all of the words in the query.
    /// Returns absolute verse numbers in canonical order.
    /// </summary>
    public static IReadOnlyList<int> Search(string query)
    {
        var words = TokenizeQuery(query);
        if (words.Count == 0)
            return Array.Empty<int>();

        var index = _index.Value;

        int[]? result = null;
        foreach (var word in words)
        {
            var wordIndex = Array.BinarySearch(Constants.Words, word, StringComparer.Ordinal);
            if (wordIndex < 0)
                return Array.Empty<int>();

            var verses = index[wordIndex];
            result = result == null ? verses : Intersect(result, verses);
            if (result.Length == 0)
                return Array.Empty<int>();
        }

        return result ?? Array.Empty<int>();
    }

    private static List<string> TokenizeQuery(string query)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var words = new List<string>();
        foreach (var part in query.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var word = part.ToLowerInvariant().Trim('\'', ',', '.', '!', '?', ';', ':');
            if (word.Length > 0 && char.IsLetter(word[0]) && seen.Add(word))
                words.Add(word);
        }
        return words;
    }

    private static int[] Intersect(int[] a, int[] b)
    {
        var result = new List<int>();
        int i = 0, j = 0;
        while (i < a.Length && j < b.Length)
        {
            if (a[i] == b[j])
            {
                result.Add(a[i]);
                i++;
                j++;
            }
            else if (a[i] < b[j])
            {
                i++;
            }
            else
            {
                j++;
            }
        }
        return result.ToArray();
    }
}
