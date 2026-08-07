using AndroidX.AppCompat.App;

namespace PewBibleKjv;

[Activity(Label = "@string/settings_title")]
public class SettingsActivity : AppCompatActivity
{
    private ImageButton _themeToggleButton = null!;
    private TextView _themeStatus = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        SetContentView(Resource.Layout.Settings);

        _themeToggleButton = FindViewById<ImageButton>(Resource.Id.themeToggleButton)!;
        _themeStatus = FindViewById<TextView>(Resource.Id.settingsThemeStatus)!;
        _themeToggleButton.Click += (_, __) =>
        {
            ThemePreferences.ToggleTwoState(this);
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
        var systemTheme = ThemePreferences.GetSystemResolvedMode();
        var hasOverride = ThemePreferences.HasSavedOverride(this);

        if (activeTheme == ThemeMode.Dark)
        {
            _themeToggleButton.SetImageResource(Resource.Drawable.ic_theme_dark);
            _themeToggleButton.ContentDescription = GetString(Resource.String.theme_dark);
        }
        else
        {
            _themeToggleButton.SetImageResource(Resource.Drawable.ic_theme_light);
            _themeToggleButton.ContentDescription = GetString(Resource.String.theme_light);
        }

        _themeStatus.Text = hasOverride
            ? GetString(Resource.String.settings_theme_status_override,
                ThemeLabel(activeTheme), ThemeLabel(systemTheme))
            : GetString(Resource.String.settings_theme_status_system, ThemeLabel(systemTheme));
    }

    private string ThemeLabel(ThemeMode mode)
    {
        return mode switch
        {
            ThemeMode.Dark => GetString(Resource.String.theme_dark)!,
            _ => GetString(Resource.String.theme_light)!,
        };
    }
}
