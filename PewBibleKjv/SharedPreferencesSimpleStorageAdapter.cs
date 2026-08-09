using Android.Content;
using PewBibleKjv.Logic.Adapters.Services;

namespace PewBibleKjv;

public sealed class SharedPreferencesSimpleStorageAdapter : ISimpleStorage
{
    private readonly ISharedPreferences _preferences;

    public SharedPreferencesSimpleStorageAdapter(ISharedPreferences preferences)
    {
        _preferences = preferences;
    }

    public void Clear(string key) => _preferences.Edit()!.Remove(key)!.Commit();

    public string Load(string key) => _preferences.GetString(key, null)!;

    public void Save(string key, string value) => _preferences.Edit()!.PutString(key, value)!.Commit();

    public int LoadInt(string key) => _preferences.GetInt(key, 0);

    public void SaveInt(string key, int value) => _preferences.Edit()!.PutInt(key, value)!.Commit();
}