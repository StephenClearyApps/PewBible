using Android.Content;
using Android.Content.Res;
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

    private static ThemeMode GetSelectedMode(Context context)
    {
        var savedOverride = GetSavedOverride(context);
        return savedOverride ?? ThemeMode.System;
    }

    public static ThemeMode GetResolvedActiveMode(Context context)
    {
        var savedOverride = GetSavedOverride(context);
        return savedOverride ?? GetSystemResolvedMode();
    }

    public static bool IsFollowingSystem(Context context) => GetSavedOverride(context) == null;

    private static ThemeMode GetSystemResolvedMode()
    {
        var nightMode = Resources.System!.Configuration!.UiMode & UiMode.NightMask;
        return nightMode == UiMode.NightYes ? ThemeMode.Dark : ThemeMode.Light;
    }

    public static void SetTwoStateMode(Context context, ThemeMode mode)
    {
        if (mode != ThemeMode.Dark && mode != ThemeMode.Light)
            throw new ArgumentOutOfRangeException(nameof(mode));

        var systemMode = GetSystemResolvedMode();
        if (mode == systemMode)
            mode = ThemeMode.System;

        SaveOverride(context, mode);
        AppCompatDelegate.DefaultNightMode = ToNightMode(mode);
    }

    private static ThemeMode? GetSavedOverride(Context context)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        if (!preferences.Contains(ThemeModePreferenceKey))
            return null;
        var selectedModeValue = preferences.GetInt(ThemeModePreferenceKey, (int)ThemeMode.System);
        if (!Enum.IsDefined(typeof(ThemeMode), selectedModeValue))
            return null;
        var selectedMode = (ThemeMode)selectedModeValue;
        if (selectedMode == ThemeMode.Dark || selectedMode == ThemeMode.Light)
            return selectedMode;
        return null;
    }

    private static void SaveOverride(Context context, ThemeMode mode)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        if (mode == ThemeMode.System)
            preferences.Edit()!.Remove(ThemeModePreferenceKey)!.Commit();
        else
            preferences.Edit()!.PutInt(ThemeModePreferenceKey, (int)mode)!.Commit();
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
