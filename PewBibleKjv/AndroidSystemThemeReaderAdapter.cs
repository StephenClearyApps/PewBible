using Android.Content.Res;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.Services;

namespace PewBibleKjv;

public sealed class AndroidSystemThemeReaderAdapter(Resources resources) : ISystemThemeReader
{
    private readonly Resources _resources = resources;

    public ThemeMode GetSystemResolvedMode()
    {
        var nightMode = _resources.Configuration!.UiMode & UiMode.NightMask;
        return nightMode == UiMode.NightYes ? ThemeMode.Dark : ThemeMode.Light;
    }
}
