using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PewBibleKjv.VerseView
{
    /// <summary>
    /// Crazy simple cache that has no expiration policy.
    /// </summary>
    /// <typeparam name="T">The type of objects contained in the cache.</typeparam>
    public sealed class SimpleCache<T>
    {
        private readonly Func<T> _create;
        private readonly List<T> _cache;

        public SimpleCache(Func<T> create)
        {
            _create = create;
            _cache = new List<T>();
        }

        public T Alloc()
        {
            if (_cache.Count != 0)
            {
                var result = _cache[_cache.Count - 1];
                _cache.RemoveAt(_cache.Count - 1);
                return result;
            }

            return _create();
        }

        public void Free(T instance) => _cache.Add(instance);
    }
}