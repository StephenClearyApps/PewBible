using Android.Content;
using Android.Content.Res;
using AndroidX.AppCompat.App;

namespace PewBibleKjv;

public enum ThemeMode
{
    SystemDefault = 0,
    Unspecified = 0,
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
        var savedOverride = GetSavedOverride(context);
        return savedOverride ?? ThemeMode.Unspecified;
    }

    public static bool HasSavedOverride(Context context) => GetSavedOverride(context).HasValue;

    public static ThemeMode GetResolvedActiveMode(Context context)
    {
        var savedOverride = GetSavedOverride(context);
        return savedOverride ?? GetSystemResolvedMode();
    }

    public static ThemeMode GetSystemResolvedMode()
    {
        var nightMode = Resources.System!.Configuration!.UiMode & UiMode.NightMask;
        return nightMode == UiMode.NightYes ? ThemeMode.Dark : ThemeMode.Light;
    }

    public static ThemeMode ToggleTwoState(Context context)
    {
        var savedOverride = GetSavedOverride(context);
        var systemMode = GetSystemResolvedMode();
        if (!savedOverride.HasValue)
        {
            var selectedOverride = Opposite(systemMode);
            SaveOverride(context, selectedOverride);
            AppCompatDelegate.DefaultNightMode = ToNightMode(selectedOverride);
            return selectedOverride;
        }

        var toggledMode = Opposite(savedOverride.Value);
        if (toggledMode == systemMode)
        {
            ClearOverride(context);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
            return systemMode;
        }

        SaveOverride(context, toggledMode);
        AppCompatDelegate.DefaultNightMode = ToNightMode(toggledMode);
        return toggledMode;
    }

    public static ThemeMode SetTwoStateMode(Context context, ThemeMode mode)
    {
        if (mode != ThemeMode.Dark && mode != ThemeMode.Light)
            throw new ArgumentOutOfRangeException(nameof(mode));

        var systemMode = GetSystemResolvedMode();
        if (mode == systemMode)
        {
            ClearOverride(context);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
            return systemMode;
        }

        SaveOverride(context, mode);
        AppCompatDelegate.DefaultNightMode = ToNightMode(mode);
        return mode;
    }

    public static void SetSelectedMode(Context context, ThemeMode mode)
    {
        if (mode == ThemeMode.SystemDefault || mode == ThemeMode.Unspecified || mode == ThemeMode.System)
        {
            ClearOverride(context);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
            return;
        }

        SaveOverride(context, mode);
        AppCompatDelegate.DefaultNightMode = ToNightMode(mode);
    }

    private static ThemeMode Opposite(ThemeMode mode)
    {
        return mode switch
        {
            ThemeMode.Dark => ThemeMode.Light,
            _ => ThemeMode.Dark,
        };
    }

    private static ThemeMode? GetSavedOverride(Context context)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        if (!preferences.Contains(ThemeModePreferenceKey))
            return null;
        var selectedModeValue = preferences.GetInt(ThemeModePreferenceKey, (int)ThemeMode.SystemDefault);
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
        preferences.Edit()!.PutInt(ThemeModePreferenceKey, (int)mode)!.Commit();
    }

    private static void ClearOverride(Context context)
    {
        var preferences = context.ApplicationContext!.GetSharedPreferences(PreferencesName, FileCreationMode.Private)!;
        preferences.Edit()!.Remove(ThemeModePreferenceKey)!.Commit();
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
