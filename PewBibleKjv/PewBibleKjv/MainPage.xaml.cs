using System;
using System.Collections.Generic;
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
	        public Label Label { get; }

	        public TestViewHolder(Label label, Android.Views.View view) : base(view)
	        {
	            Label = label;
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
	            vh.Label.Text = _data[position].ToString();
	        }

	        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
	        {
	            var label = new Label { Text = "Hello!" };
	            var cv = new ContentView
	            {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
	                Content = label,
	            };
	            var renderer = Xamarin.Forms.Platform.Android.Platform.CreateRendererWithContext(cv, parent.Context);
	            var view = renderer.View;
	            renderer.Tracker.UpdateLayout();
                view.LayoutParameters = new RecyclerView.LayoutParams(parent.Width, parent.Height);
	            cv.Layout(Rectangle.FromLTRB(0, 0, parent.Width, parent.Height));
                view.Layout(0, 0, (int)cv.Width, (int)cv.Height);

                return new TestViewHolder(label, view);
	        }

	        public override int ItemCount => _data.Count;
	    }
    }
}
