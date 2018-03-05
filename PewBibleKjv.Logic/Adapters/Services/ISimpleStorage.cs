using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Logic.Adapters.Services
{
    /// <summary>
    /// A simple key/value store for strings.
    /// </summary>
    public interface ISimpleStorage
    {
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
    }
}
