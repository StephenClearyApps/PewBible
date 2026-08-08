using AndroidX.AppCompat.App;

namespace PewBibleKjv;

[Activity(Label = "@string/settings_title")]
public class SettingsActivity : AppCompatActivity
{
    private ImageButton _themeToggleButton = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        SetContentView(Resource.Layout.Settings);

        _themeToggleButton = FindViewById<ImageButton>(Resource.Id.themeToggleButton)!;
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
    }
}
