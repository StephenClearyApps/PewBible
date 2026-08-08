using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

namespace PewBibleKjv;

[Activity(Label = "@string/settings_title")]
public class SettingsActivity : AppCompatActivity
{
    private SwitchCompat _themeToggleSwitch = null!;
    private bool _refreshingThemeUi;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        SetContentView(Resource.Layout.Settings);

        _themeToggleSwitch = FindViewById<SwitchCompat>(Resource.Id.themeToggleButton)!;
        _themeToggleSwitch.CheckedChange += (_, args) =>
        {
            if (_refreshingThemeUi)
                return;

            var selectedMode = args.IsChecked ? ThemeMode.Dark : ThemeMode.Light;
            ThemePreferences.SetTwoStateMode(this, selectedMode);
            RefreshThemeUi();
        };

        RefreshThemeUi();
    }

    protected override void OnResume()
    {
        base.OnResume();
        RefreshThemeUi();
    }

    private void RefreshThemeUi()
    {
        var activeTheme = ThemePreferences.GetResolvedActiveMode(this);
        _refreshingThemeUi = true;
        _themeToggleSwitch.Checked = activeTheme == ThemeMode.Dark;
        _refreshingThemeUi = false;

        if (activeTheme == ThemeMode.Dark)
        {
            _themeToggleSwitch.ContentDescription = GetString(Resource.String.theme_dark);
        }
        else
        {
            _themeToggleSwitch.ContentDescription = GetString(Resource.String.theme_light);
        }
    }
}
