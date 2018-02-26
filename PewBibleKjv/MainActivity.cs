using System.Runtime.InteropServices;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using PewBibleKjv.ViewModels;
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

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(new TestAdapter(new TestData()));
            recyclerView.AddOnScrollListener(new ScrollListener(layoutManager));
            recyclerView.ScrollToPosition(1000);
        }

        public class ScrollListener : RecyclerView.OnScrollListener
        {
            private readonly LinearLayoutManager _layoutManager;

            public ScrollListener(LinearLayoutManager layoutManager)
            {
                _layoutManager = layoutManager;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                var firstIndex = _layoutManager.FindFirstVisibleItemPosition();
                Debug.WriteLine("Scrolled to position: " + firstIndex);
                var view = (TestViewHolder)recyclerView.FindViewHolderForLayoutPosition(firstIndex);
                Debug.WriteLine("Scrolled to verse: " + view.Verse);
            }
        }

        public class TestViewHolder : RecyclerView.ViewHolder
        {
            public TextView View { get; }
            public int Verse { get; set; }

            public TestViewHolder(TextView view) : base(view)
            {
                View = view;
            }
        }

        public class TestAdapter : RecyclerView.Adapter
        {
            private readonly TestData _data;

            public TestAdapter(TestData data)
            {
                _data = data;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var vh = (TestViewHolder)holder;
                vh.Verse = position;
                vh.View.Text = _data[position];
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                return new TestViewHolder(new TextView(parent.Context));
            }

            public override int ItemCount => _data.Count;
        }
    }
}

