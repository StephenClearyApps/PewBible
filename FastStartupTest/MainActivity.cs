using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using PewBibleKjv.ViewModels;

namespace FastStartupTest
{
    [Activity(Label = "FastStartupTest", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(new TestAdapter(new TestData()));
            recyclerView.ScrollToPosition(1000);
        }
        public class TestViewHolder : RecyclerView.ViewHolder
        {
            public TextView View { get; }

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

