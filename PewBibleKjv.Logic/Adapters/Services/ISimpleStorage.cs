using System;
using System.Collections.Generic;
using System.Text;

namespace PewBibleKjv.Logic.Adapters.Services
{
    public interface ISimpleStorage
    {
        string Load(string key);
        void Save(string key, string value);
    }
}
