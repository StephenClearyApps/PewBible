using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PewBibleKjv.Logic.Adapters.Services;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubSimpleStorage: ISimpleStorage
    {
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public void Clear(string key) => _data.Remove(key);

        public string Load(string key)
        {
            _data.TryGetValue(key, out var result);
            return result;
        }

        public int LoadInt(string key)
        {
            _data.TryGetValue(key, out var result);
            return int.TryParse(result, out var intValue) ? intValue : 0;
        }

        public void Save(string key, string value) => _data[key] = value;

        public void SaveInt(string key, int value) => _data[key] = value.ToString();
    }
}
