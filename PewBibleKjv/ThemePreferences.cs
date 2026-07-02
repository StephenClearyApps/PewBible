using Android.Content;
using AndroidX.AppCompat.App;

namespace PewBibleKjv;

public enum ThemeMode
{
    System = 0,
    Light = 1,
    Dark = 2,
}

public static class ThemePreferences
{
    private const string PreferencesName = "global";
    private const string ThemeModePreferenceKey = "ThemeMode";

    public static void ApplyFromPreferences(Context context)
    {
        var selectedMode = GetSelectedMode(context);
        AppCompatDelegate.DefaultNightMode = ToNightMode(selectedMode);
    }

    public static ThemeMode GetSelectedMode(Context context)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        var selectedModeValue = preferences.GetInt(ThemeModePreferenceKey, (int)ThemeMode.System);
        return Enum.IsDefined(typeof(ThemeMode), selectedModeValue) ? (ThemeMode)selectedModeValue : ThemeMode.System;
    }

    public static void SetSelectedMode(Context context, ThemeMode mode)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        preferences.Edit()!.PutInt(ThemeModePreferenceKey, (int)mode)!.Commit();
        AppCompatDelegate.DefaultNightMode = ToNightMode(mode);
    }

    private static int ToNightMode(ThemeMode mode)
    {
        return mode switch
        {
            ThemeMode.Light => AppCompatDelegate.ModeNightNo,
            ThemeMode.Dark => AppCompatDelegate.ModeNightYes,
            _ => AppCompatDelegate.ModeNightFollowSystem,
        };
    }
}
