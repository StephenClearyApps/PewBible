using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PewBibleKjv.Logic.Adapters.Services;

namespace UnitTests.Util
{
    [ExcludeFromCodeCoverage]
    public sealed class StubSimpleStorage: ISimpleStorage
    {
        private readonly Dictionary<string, string> _data = [];

        public string Load(string key)
        {
            _data.TryGetValue(key, out var result);
            return result;
        }

        public void Save(string key, string value) => _data[key] = value;
    }
}
