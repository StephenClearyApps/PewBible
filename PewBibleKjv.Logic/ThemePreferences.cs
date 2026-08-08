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
        var selectedMode = GetSavedOverride();
        _themeModeApplier.Apply(selectedMode);
    }

    public ThemeMode GetResolvedActiveMode()
    {
        var savedOverride = GetSavedOverride();
        return savedOverride == ThemeMode.System ? _systemThemeReader.GetSystemResolvedMode() : savedOverride;
    }

    public bool IsFollowingSystem() => GetSavedOverride() == ThemeMode.System;

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

    private ThemeMode GetSavedOverride()
    {
        var rawValue = _simpleStorage.LoadInt(ThemeModePreferenceKey);
        if (!Enum.IsDefined(typeof(ThemeMode), rawValue))
            return ThemeMode.System;
        return (ThemeMode)rawValue;
    }

    private void SaveOverride(ThemeMode mode)
    {
        if (mode == ThemeMode.System)
            _simpleStorage.Clear(ThemeModePreferenceKey);
        else
            _simpleStorage.SaveInt(ThemeModePreferenceKey, (int)mode);
    }
}
