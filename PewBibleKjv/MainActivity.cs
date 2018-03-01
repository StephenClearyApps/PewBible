using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using PewBibleKjv.Logic;
using PewBibleKjv.Text;
using Debug = System.Diagnostics.Debug;

namespace PewBibleKjv
{
    [Activity(Label = "PewBibleKjv", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private App _app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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

            // Wire up events
            var chapterHeading = FindViewById<Button>(Resource.Id.headingText);
            chapterHeading.Click += (_, __) => StartActivity(typeof(ChooseBookActivity));

            // Initialize the app
            var chapterHeadingAdapter = new TextViewChapterHeadingAdapter(chapterHeading);
            var verseViewAdapter = new RecyclerViewVerseViewAdapter(recyclerView, layoutManager, position =>
            {
                var view = (VerseViewHolder)recyclerView.FindViewHolderForLayoutPosition(position);
                return view.Location;
            });
            var simpleStorageAdapter = new SharedPreferencesSimpleStorageAdapter(ApplicationContext.GetSharedPreferences("global", FileCreationMode.Private));
            _app = new App(chapterHeadingAdapter, verseViewAdapter, simpleStorageAdapter, startingVerse);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _app.Pause();
        }

        public class VerseViewHolder : RecyclerView.ViewHolder
        {
            public TextView View { get; }
            public Location Location { get; set; }

            public VerseViewHolder(View view) : base(view)
            {
                View = view.FindViewById<TextView>(Resource.Id.verseText);
            }
        }

        public class VerseAdapter : RecyclerView.Adapter
        {
            private readonly TextService _data;
            private readonly LayoutInflater _layoutInflater;

            public VerseAdapter(TextService data, LayoutInflater layoutInflater)
            {
                _data = data;
                _layoutInflater = layoutInflater;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var vh = (VerseViewHolder)holder;
                vh.Location = _data[position];
                vh.View.Text = Bible.FormattedVerse(vh.Location.AbsoluteVerseNumber).Text;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var verseView = _layoutInflater.Inflate(Resource.Layout.VerseLayout, parent, attachToRoot: false);
                return new VerseViewHolder(verseView);
            }

            public override int ItemCount => _data.Count;
        }
    }
}

