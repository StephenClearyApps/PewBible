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
using PewBibleKjv.Logic.Adapters.Services;

namespace PewBibleKjv
{
    public sealed class SharedPreferencesSimpleStorageAdapter : ISimpleStorage
    {
        public string Load(string key)
        {
            return null;
        }

        public void Save(string key, string value)
        {
        }
    }
}