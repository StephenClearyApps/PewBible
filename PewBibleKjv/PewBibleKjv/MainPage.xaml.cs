using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PewBibleKjv.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Xaml;

namespace PewBibleKjv
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		    var recyclerView = GetNative<RecyclerView>(RecyclerViewParent);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Android.App.Application.Context));
            recyclerView.SetAdapter(new TestAdapter(new TestData()));
            recyclerView.ScrollToPosition(1000);
        }

	    private static T GetNative<T>(ContentView contentView) where T : Android.Views.View
	    {
	        var wrapper = (NativeViewWrapper)contentView.Content;
	        return (T)wrapper.NativeView;
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
