using Android.Content.PM;
using Android.Content.Res;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Android.Views;

namespace PewBibleKjv;

[Activity(Label = "@string/settings_title", ConfigurationChanges = ConfigChanges.UiMode)]
public class SettingsActivity : AppCompatActivity
{
    private View _themeRow = null!;
    private SwitchCompat _themeToggleSwitch = null!;
    private bool _refreshingThemeUi;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        SetContentView(Resource.Layout.Settings);

        _themeRow = FindViewById<View>(Resource.Id.settingsThemeRow)!;
        _themeToggleSwitch = FindViewById<SwitchCompat>(Resource.Id.themeToggleButton)!;
        _themeRow.Click += (_, __) =>
        {
            _themeToggleSwitch.Checked = !_themeToggleSwitch.Checked;
        };
        _themeToggleSwitch.CheckedChange += (_, args) =>
        {
            if (_refreshingThemeUi)
                return;

            var selectedMode = args.IsChecked ? ThemeMode.Light : ThemeMode.Dark;
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

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        if (!ThemePreferences.IsFollowingSystem(this))
            return;
        ThemePreferences.ApplyFromPreferences(this);
        Recreate();
    }

    private void RefreshThemeUi()
    {
        var activeTheme = ThemePreferences.GetResolvedActiveMode(this);
        _refreshingThemeUi = true;
        _themeToggleSwitch.Checked = activeTheme == ThemeMode.Light;
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
