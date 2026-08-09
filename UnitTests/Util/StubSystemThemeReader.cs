using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;

namespace UnitTests.Util;

public sealed class StubSystemThemeReader : ISystemThemeReader
{
    public ThemeMode SystemMode { get; set; } = ThemeMode.Light;

    public ThemeMode GetSystemResolvedMode() => SystemMode;
}
