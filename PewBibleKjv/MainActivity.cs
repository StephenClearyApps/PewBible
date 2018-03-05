using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
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
            recyclerView.SetAdapter(new VerseAdapter(new TextService(), LayoutInflater));

            // Wire up Android-side events
            var chapterHeading = FindViewById<Button>(Resource.Id.headingText);
            chapterHeading.Click += (_, __) => StartActivity(typeof(ChooseBookActivity));

            // Initialize the app
            var chapterHeadingAdapter = new TextViewChapterHeadingAdapter(chapterHeading);
            var verseViewAdapter = new RecyclerViewVerseViewAdapter(recyclerView, layoutManager);
            var simpleStorageAdapter = new SharedPreferencesSimpleStorageAdapter(ApplicationContext.GetSharedPreferences("global", FileCreationMode.Private));
            _backButton = FindViewById<ImageButton>(Resource.Id.backButton);
            _forwardButton = FindViewById<ImageButton>(Resource.Id.forwardButton);
            var historyControlsAdapter = new ViewHistoryControlsAdapter(_backButton, _forwardButton);
            _app = new App(chapterHeadingAdapter, verseViewAdapter, simpleStorageAdapter, historyControlsAdapter, startingVerse);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _app.Dispose();
        }
    }
}

