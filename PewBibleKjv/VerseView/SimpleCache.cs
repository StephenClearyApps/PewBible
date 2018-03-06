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
        private readonly List<Item> _cache;

        public SimpleCache(Func<T> create)
        {
            _create = create;
            _cache = new List<Item>();
        }

        public ISimpleCacheItem<T> Alloc()
        {
            if (_cache.Count != 0)
            {
                var result = _cache[_cache.Count - 1];
                _cache.RemoveAt(_cache.Count - 1);
                return result;
            }

            return new Item(this, _create());
        }

        private sealed class Item : ISimpleCacheItem<T>
        {
            private readonly SimpleCache<T> _cache;

            public Item(SimpleCache<T> cache, T instance)
            {
                _cache = cache;
                Instance = instance;
            }

            public T Instance { get; }

            public void Free() => _cache._cache.Add(this);
        }
    }
}