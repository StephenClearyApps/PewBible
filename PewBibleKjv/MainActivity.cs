using System.Diagnostics;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Views;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using PewBibleKjv.Util;
using PewBibleKjv.VerseView;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.Activity;

namespace PewBibleKjv;

[Activity(Label = "Pew Bible (KJV)", MainLauncher = true, Icon = "@mipmap/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.UiMode)]
public class MainActivity : AppCompatActivity
{
    private CoreApp _app = null!;
    private ImageButton _backButton = null!;
    private ImageButton _forwardButton = null!;
    private TextViewChapterHeadingAdapter _chapterHeadingAdapter = null!;
    private RecyclerViewVerseViewAdapter _verseViewAdapter = null!;
    private SharedPreferencesSimpleStorageAdapter _simpleStorageAdapter = null!;
    private ViewHistoryControlsAdapter _historyControlsAdapter = null!;
    private OnBackPressedCallback _customBackCallback = null!;
    private DrawerLayout _drawerLayout = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);

        SetContentView(Resource.Layout.Main);

        UpdateStatusBarAppearance();

        // Set up our view
        var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView)!;
        _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout)!;
        var swipeMenuList = FindViewById<ListView>(Resource.Id.swipeMenuList)!;
        swipeMenuList.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1,
            [GetString(Resource.String.menu_settings)!]);
        swipeMenuList.ItemClick += (_, _) =>
        {
            _drawerLayout.CloseDrawer((int)GravityFlags.Start);
            StartActivity(typeof(SettingsActivity));
        };
        var layoutManager = new LinearLayoutManager(this);
        recyclerView.SetLayoutManager(layoutManager);
        recyclerView.SetAdapter(new VerseAdapter(LayoutInflater));

        // Wire up Android-side events
        var chapterHeading = FindViewById<Button>(Resource.Id.headingText)!;
        chapterHeading.Click += (_, __) => StartActivity(typeof(ChooseBookActivity));

        _customBackCallback = new CustomOnBackInvokedCallback(() =>
        {
            if (_drawerLayout.IsDrawerOpen((int)GravityFlags.Start))
            {
                _drawerLayout.CloseDrawer((int)GravityFlags.Start);
                return;
            }

            if (_backButton.Enabled)
            {
                _backButton.CallOnClick();
            }
            else
            {
                _customBackCallback.Enabled = false;
                OnBackPressedDispatcher.OnBackPressed();
                _customBackCallback.Enabled = true;
            }
        });

        OnBackPressedDispatcher.AddCallback(this, _customBackCallback);

        // Initialize the app
        _chapterHeadingAdapter = new TextViewChapterHeadingAdapter(chapterHeading);
        _verseViewAdapter = new RecyclerViewVerseViewAdapter(this, recyclerView, layoutManager, ChapterHeadingHeight());
        _simpleStorageAdapter = new SharedPreferencesSimpleStorageAdapter(ApplicationContext!.GetSharedPreferences("global", FileCreationMode.Private)!);
        _backButton = FindViewById<ImageButton>(Resource.Id.backButton)!;
        _forwardButton = FindViewById<ImageButton>(Resource.Id.forwardButton)!;
        _historyControlsAdapter = new ViewHistoryControlsAdapter(_backButton, _forwardButton);

        CreateApp();
    }

    private int ChapterHeadingHeight()
    {
        var size = AndroidUtils.MeasureLayout(this, LayoutInflater, Resource.Layout.VerseLayout,
            setupView: view =>
            {
                var chapterHeaderView = view.FindViewById<TextView>(Resource.Id.verseChapterHeaderText)!;
                var verseView = view.FindViewById<TextView>(Resource.Id.verseText)!;
                verseView.Visibility = ViewStates.Gone;
                chapterHeaderView.Text = "Chapter 150";
            });
        return size.Height;
    }

    protected override void OnNewIntent(Intent? intent)
    {
        Intent = intent;
        base.OnNewIntent(intent);
    }

    protected override void OnPause()
    {
        base.OnPause();
        _app.Dispose();
        _app = null!;
    }

    protected override void OnResume()
    {
        CreateApp();
        UpdateStatusBarAppearance();
        base.OnResume();
    }

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        if (!ThemePreferences.IsFollowingSystem(this))
            return;
        ThemePreferences.ApplyFromPreferences(this);
        Recreate();
    }

    private void UpdateStatusBarAppearance()
    {
        var insetsController = AndroidX.Core.View.WindowCompat.GetInsetsController(Window, Window!.DecorView);
        if (insetsController != null)
        {
            var isNightMode = (Resources!.Configuration!.UiMode & UiMode.NightMask) == UiMode.NightYes;
            insetsController.AppearanceLightStatusBars = !isNightMode;
        }
    }

    private void CreateApp()
    {
        if (_app != null)
            return;
        _app = new CoreApp(_chapterHeadingAdapter, _verseViewAdapter, _simpleStorageAdapter, _historyControlsAdapter, IntentStartingVerse());
    }

    private int IntentStartingVerse()
    {
        // Determine if we have an intent to go to a particular verse.
        var bookIndex = Intent!.GetIntExtra("BookIndex", -1);
        var chapterIndex = Intent.GetIntExtra("ChapterIndex", -1);
        var startingVerse = Bible.InvalidAbsoluteVerseNumber;
        if (bookIndex != -1 && chapterIndex != -1)
            startingVerse = Structure.Books[bookIndex].Chapters[chapterIndex].BeginVerse;
        return startingVerse;
    }

		private sealed class CustomOnBackInvokedCallback(Action action) : OnBackPressedCallback(enabled: true)
		{
			public override void HandleOnBackPressed() => action();
		}
	}
