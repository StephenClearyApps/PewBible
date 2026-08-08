using AndroidX.AppCompat.App;
using PewBibleKjv.Logic;
using PewBibleKjv.Logic.Adapters.UI;

namespace PewBibleKjv;

public sealed class AppCompatThemeModeApplierAdapter : IThemeModeApplier
{
    public void Apply(ThemeMode mode)
    {
        AppCompatDelegate.DefaultNightMode = mode switch
        {
            ThemeMode.Light => AppCompatDelegate.ModeNightNo,
            ThemeMode.Dark => AppCompatDelegate.ModeNightYes,
            _ => AppCompatDelegate.ModeNightFollowSystem,
        };
    }
}
