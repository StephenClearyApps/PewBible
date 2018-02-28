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
        private readonly ISharedPreferences _preferences;

        public SharedPreferencesSimpleStorageAdapter(ISharedPreferences preferences)
        {
            _preferences = preferences;
        }

        public string Load(string key) => _preferences.GetString(key, null);

        public void Save(string key, string value) => _preferences.Edit().PutString(key, value).Commit();
    }
}