using System.Runtime.InteropServices;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using PewBibleKjv.Services;
using PewBibleKjv.Text;
using Debug = System.Diagnostics.Debug;

namespace PewBibleKjv
{
    [Activity(Label = "PewBibleKjv", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var headingText = FindViewById<TextView>(Resource.Id.headingText);
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(new VerseAdapter(TextService.Instance));
            recyclerView.AddOnScrollListener(new ScrollListener(layoutManager, headingText));
            recyclerView.ScrollToPosition(1000);
        }

        public class ScrollListener : RecyclerView.OnScrollListener
        {
            private readonly LinearLayoutManager _layoutManager;
            private readonly TextView _headingText;
            private int _lastPosition = -1;

            public ScrollListener(LinearLayoutManager layoutManager, TextView headingText)
            {
                _layoutManager = layoutManager;
                _headingText = headingText;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                var firstIndex = _layoutManager.FindFirstVisibleItemPosition();
                if (firstIndex == _lastPosition)
                    return;
                _lastPosition = firstIndex;
                var view = (VerseViewHolder)recyclerView.FindViewHolderForLayoutPosition(firstIndex);
                var chapterHeadingText = view.Location.ChapterHeadingText;
                if (_headingText.Text != chapterHeadingText)
                    _headingText.Text = chapterHeadingText;
            }
        }

        public class VerseViewHolder : RecyclerView.ViewHolder
        {
            public TextView View { get; }
            public Location Location { get; set; }

            public VerseViewHolder(TextView view) : base(view)
            {
                View = view;
            }
        }

        public class VerseAdapter : RecyclerView.Adapter
        {
            private readonly TextService _data;

            public VerseAdapter(TextService data)
            {
                _data = data;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var vh = (VerseViewHolder)holder;
                vh.Location = _data[position];
                vh.View.Text = Bible.FormattedVerse(vh.Location.AbsoluteVerseNumber).Text;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                return new VerseViewHolder(new TextView(parent.Context));
            }

            public override int ItemCount => _data.Count;
        }
    }
}

