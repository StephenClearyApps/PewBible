using System.Globalization;
using PewBibleKjv.Logic.Adapters.Services;
using PewBibleKjv.Logic.Adapters.UI;

namespace PewBibleKjv.Logic;

public sealed class ThemePreferences
{
    private const string ThemeModePreferenceKey = "ThemeMode";

    private readonly ISimpleStorage _simpleStorage;
    private readonly ISystemThemeReader _systemThemeReader;
    private readonly IThemeModeApplier _themeModeApplier;

    public ThemePreferences(ISimpleStorage simpleStorage, ISystemThemeReader systemThemeReader, IThemeModeApplier themeModeApplier)
    {
        _simpleStorage = simpleStorage;
        _systemThemeReader = systemThemeReader;
        _themeModeApplier = themeModeApplier;
    }

    public void ApplyFromPreferences()
    {
        var selectedMode = GetSelectedMode();
        _themeModeApplier.Apply(selectedMode);
    }

    public ThemeMode GetResolvedActiveMode()
    {
        var savedOverride = GetSavedOverride();
        return savedOverride ?? _systemThemeReader.GetSystemResolvedMode();
    }

    public bool IsFollowingSystem() => GetSavedOverride() == null;

    public void SetTwoStateMode(ThemeMode mode)
    {
        if (mode != ThemeMode.Dark && mode != ThemeMode.Light)
            throw new ArgumentOutOfRangeException(nameof(mode));

        var systemMode = _systemThemeReader.GetSystemResolvedMode();
        if (mode == systemMode)
            mode = ThemeMode.System;

        SaveOverride(mode);
        _themeModeApplier.Apply(mode);
    }

    public bool HandleSystemThemeChanged()
    {
        if (!IsFollowingSystem())
            return false;
        _themeModeApplier.Apply(ThemeMode.System);
        return true;
    }

    private ThemeMode GetSelectedMode()
    {
        var savedOverride = GetSavedOverride();
        return savedOverride ?? ThemeMode.System;
    }

    private ThemeMode? GetSavedOverride()
    {
        var rawValue = _simpleStorage.Load(ThemeModePreferenceKey);
        if (rawValue == null)
            return null;

        if (int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) &&
            Enum.IsDefined(typeof(ThemeMode), value))
        {
            var parsed = (ThemeMode)value;
            if (parsed == ThemeMode.Light || parsed == ThemeMode.Dark)
                return parsed;
            return null;
        }

        if (!Enum.TryParse<ThemeMode>(rawValue, ignoreCase: true, out var enumValue))
            return null;
        if (enumValue == ThemeMode.Light || enumValue == ThemeMode.Dark)
            return enumValue;
        return null;
    }

    private void SaveOverride(ThemeMode mode)
    {
        if (mode == ThemeMode.System)
            _simpleStorage.Save(ThemeModePreferenceKey, ThemeMode.System.ToString());
        else
            _simpleStorage.Save(ThemeModePreferenceKey, mode.ToString());
    }
}
