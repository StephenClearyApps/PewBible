using Android.Content;
using PewBibleKjv.Logic;
using LogicThemePreferences = PewBibleKjv.Logic.ThemePreferences;

namespace PewBibleKjv;

public static class ThemePreferences
{
    private const string PreferencesName = "global";
    private const string ThemeModePreferenceKey = "ThemeMode";

    public static void ApplyFromPreferences(Context context)
    {
        Build(context).ApplyFromPreferences();
    }

    public static ThemeMode GetResolvedActiveMode(Context context)
    {
        return Build(context).GetResolvedActiveMode();
    }

    public static bool IsFollowingSystem(Context context) => Build(context).IsFollowingSystem();

    public static void SetTwoStateMode(Context context, ThemeMode mode)
    {
        Build(context).SetTwoStateMode(mode);
    }

    public static bool HandleSystemThemeChanged(Context context)
    {
        return Build(context).HandleSystemThemeChanged();
    }

    private static LogicThemePreferences Build(Context context)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        MigrateLegacyThemePreference(preferences);
        var simpleStorage = new SharedPreferencesSimpleStorageAdapter(preferences);
        var systemThemeReader = new AndroidSystemThemeReaderAdapter(context.Resources!);
        var themeModeApplier = new AppCompatThemeModeApplierAdapter();
        return new LogicThemePreferences(simpleStorage, systemThemeReader, themeModeApplier);
    }

    private static void MigrateLegacyThemePreference(ISharedPreferences preferences)
    {
        if (!preferences.Contains(ThemeModePreferenceKey))
            return;
        try
        {
            preferences.GetString(ThemeModePreferenceKey, null);
        }
        catch (Java.Lang.ClassCastException)
        {
            var value = preferences.GetInt(ThemeModePreferenceKey, (int)ThemeMode.System);
            preferences.Edit()!
                .Remove(ThemeModePreferenceKey)!
                .PutString(ThemeModePreferenceKey, ((ThemeMode)value).ToString())!
                .Commit();
        }
    }
}
