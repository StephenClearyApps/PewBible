using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBibleKjv.Logic.Adapters.Services;

namespace UnitTests.Util
{
    public sealed class StubSimpleStorage: ISimpleStorage
    {
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public string Load(string key)
        {
            _data.TryGetValue(key, out var result);
            return result;
        }

        public void Save(string key, string value) => _data[key] = value;
    }
}
