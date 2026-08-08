using PewBibleKjv.Logic;
using UnitTests.Util;
using Xunit;

namespace UnitTests;

public sealed class ThemePreferencesUnitTests
{
    [Fact]
    public void ApplyFromPreferences_WithNoOverride_AppliesSystemMode()
    {
        var (sut, _, applier) = CreateSut(systemMode: ThemeMode.Dark);

        sut.ApplyFromPreferences();

        Assert.Equal(ThemeMode.System, applier.LastAppliedMode);
    }

    [Fact]
    public void GetResolvedActiveMode_WithNoOverride_UsesSystemMode()
    {
        var (sut, _, _) = CreateSut(systemMode: ThemeMode.Dark);

        var resolved = sut.GetResolvedActiveMode();

        Assert.Equal(ThemeMode.Dark, resolved);
    }

    [Fact]
    public void GetResolvedActiveMode_WithExplicitOverride_UsesOverride()
    {
        var (sut, storage, _) = CreateSut(systemMode: ThemeMode.Light);
        storage.Save("ThemeMode", ThemeMode.Dark.ToString());

        var resolved = sut.GetResolvedActiveMode();

        Assert.Equal(ThemeMode.Dark, resolved);
    }

    [Fact]
    public void SetTwoStateMode_WhenMatchingSystem_SavesSystemAndFollowsSystem()
    {
        var (sut, storage, applier) = CreateSut(systemMode: ThemeMode.Light);

        sut.SetTwoStateMode(ThemeMode.Light);

        Assert.Equal(ThemeMode.System.ToString(), storage.Load("ThemeMode"));
        Assert.True(sut.IsFollowingSystem());
        Assert.Equal(ThemeMode.System, applier.LastAppliedMode);
    }

    [Fact]
    public void SetTwoStateMode_WhenDifferentFromSystem_SavesOverride()
    {
        var (sut, storage, applier) = CreateSut(systemMode: ThemeMode.Light);

        sut.SetTwoStateMode(ThemeMode.Dark);

        Assert.Equal(ThemeMode.Dark.ToString(), storage.Load("ThemeMode"));
        Assert.False(sut.IsFollowingSystem());
        Assert.Equal(ThemeMode.Dark, applier.LastAppliedMode);
    }

    [Fact]
    public void SetTwoStateMode_WithSystem_Throws()
    {
        var (sut, _, _) = CreateSut(systemMode: ThemeMode.Light);

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetTwoStateMode(ThemeMode.System));
    }

    [Fact]
    public void GetResolvedActiveMode_WithLegacyNumericValue_ParsesLegacyValue()
    {
        var (sut, storage, _) = CreateSut(systemMode: ThemeMode.Light);
        storage.Save("ThemeMode", ((int)ThemeMode.Dark).ToString());

        var resolved = sut.GetResolvedActiveMode();

        Assert.Equal(ThemeMode.Dark, resolved);
    }

    [Fact]
    public void HandleSystemThemeChanged_WhenFollowingSystem_AppliesAndReturnsTrue()
    {
        var (sut, _, applier) = CreateSut(systemMode: ThemeMode.Light);

        var handled = sut.HandleSystemThemeChanged();

        Assert.True(handled);
        Assert.Equal(ThemeMode.System, applier.LastAppliedMode);
    }

    [Fact]
    public void HandleSystemThemeChanged_WhenNotFollowingSystem_ReturnsFalse()
    {
        var (sut, _, applier) = CreateSut(systemMode: ThemeMode.Light);
        sut.SetTwoStateMode(ThemeMode.Dark);
        var beforeCount = applier.ApplyCount;

        var handled = sut.HandleSystemThemeChanged();

        Assert.False(handled);
        Assert.Equal(beforeCount, applier.ApplyCount);
    }

    private static (ThemePreferences sut, StubSimpleStorage storage, StubThemeModeApplier applier) CreateSut(ThemeMode systemMode)
    {
        var storage = new StubSimpleStorage();
        var systemThemeReader = new StubSystemThemeReader { SystemMode = systemMode };
        var applier = new StubThemeModeApplier();
        return (new ThemePreferences(storage, systemThemeReader, applier), storage, applier);
    }
}
