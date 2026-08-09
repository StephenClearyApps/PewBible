namespace PewBibleKjv.Logic.Adapters.Services;

/// <summary>
/// A simple key/value store for strings.
/// </summary>
public interface ISimpleStorage
{
    /// <summary>
    /// Clears a value from storage.
    /// </summary>
    /// <param name="key">The key for the value to clear.</param>
    void Clear(string key);

    /// <summary>
    /// Attempts to load a value from storage. Returns <c>null</c> if <paramref name="key"/> is not found.
    /// </summary>
    /// <param name="key">The key for the value to return.</param>
    string Load(string key);

    /// <summary>
    /// Saves a value to storage.
    /// </summary>
    /// <param name="key">The key for the value.</param>
    /// <param name="value">The value.</param>
    void Save(string key, string value);

    /// <summary>
    /// Attempts to load a value from storage. Returns <c>0</c> if <paramref name="key"/> is not found.
    /// </summary>
    /// <param name="key">The key for the value to return.</param>
    int LoadInt(string key);

    /// <summary>
    /// Saves a value to storage.
    /// </summary>
    /// <param name="key">The key for the value.</param>
    /// <param name="value">The value.</param>
    void SaveInt(string key, int value);
}
