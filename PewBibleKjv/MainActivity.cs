using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using PewBibleKjv.VerseView;

namespace PewBibleKjv
{
    [Activity(Label = "PewBibleKjv", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        private App _app;
        private ImageButton _backButton;
        private ImageButton _forwardButton;
        private TextViewChapterHeadingAdapter _chapterHeadingAdapter;
        private RecyclerViewVerseViewAdapter _verseViewAdapter;
        private SharedPreferencesSimpleStorageAdapter _simpleStorageAdapter;
        private ViewHistoryControlsAdapter _historyControlsAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Determine if we have an intent to go to a particular verse.
            var bookIndex = Intent.GetIntExtra("BookIndex", -1);
            var chapterIndex = Intent.GetIntExtra("ChapterIndex", -1);
            var startingVerse = Bible.InvalidAbsoluteVerseNumber;
            if (bookIndex != -1 && chapterIndex != -1)
                startingVerse = Structure.Books[bookIndex].Chapters[chapterIndex].BeginVerse;

            // Set up our view
            SetContentView(Resource.Layout.Main);
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(new VerseAdapter(LayoutInflater));

            // Wire up Android-side events
            var chapterHeading = FindViewById<Button>(Resource.Id.headingText);
            chapterHeading.Click += (_, __) => StartActivity(typeof(ChooseBookActivity));

            // Initialize the app
            _chapterHeadingAdapter = new TextViewChapterHeadingAdapter(chapterHeading);
            _verseViewAdapter = new RecyclerViewVerseViewAdapter(this, recyclerView, layoutManager);
            _simpleStorageAdapter = new SharedPreferencesSimpleStorageAdapter(ApplicationContext.GetSharedPreferences("global", FileCreationMode.Private));
            _backButton = FindViewById<ImageButton>(Resource.Id.backButton);
            _forwardButton = FindViewById<ImageButton>(Resource.Id.forwardButton);
            _historyControlsAdapter = new ViewHistoryControlsAdapter(_backButton, _forwardButton);

            CreateApp(startingVerse);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _app.Dispose();
            _app = null;
        }

        protected override void OnResume()
        {
            base.OnResume();
            CreateApp(Bible.InvalidAbsoluteVerseNumber);
        }

        private void CreateApp(int startingVerse)
        {
            if (_app != null)
                return;
            _app = new App(_chapterHeadingAdapter, _verseViewAdapter, _simpleStorageAdapter, _historyControlsAdapter, startingVerse);
        }
    }
}

