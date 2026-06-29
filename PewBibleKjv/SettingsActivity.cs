using AndroidX.AppCompat.App;

namespace PewBibleKjv;

[Activity(Label = "@string/settings_title")]
public class SettingsActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        SetContentView(Resource.Layout.Settings);

        var themeModeGroup = FindViewById<RadioGroup>(Resource.Id.themeModeGroup)!;
        var currentThemeMode = ThemePreferences.GetSelectedMode(this);
        themeModeGroup.Check(currentThemeMode switch
        {
            ThemeMode.Light => Resource.Id.themeModeLight,
            ThemeMode.Dark => Resource.Id.themeModeDark,
            _ => Resource.Id.themeModeSystem,
        });

        themeModeGroup.CheckedChange += (_, args) =>
        {
            var selectedThemeMode = args.CheckedId switch
            {
                Resource.Id.themeModeLight => ThemeMode.Light,
                Resource.Id.themeModeDark => ThemeMode.Dark,
                _ => ThemeMode.System,
            };
            ThemePreferences.SetSelectedMode(this, selectedThemeMode);
        };
    }
}
