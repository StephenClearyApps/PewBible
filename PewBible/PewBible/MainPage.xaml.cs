using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewBible.ViewModels;
using Xamarin.Forms;

namespace PewBible
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		    ListView1.ItemsSource = new TestData();
            ListView1.ScrollTo(1000, ScrollToPosition.MakeVisible, animated: false);
		}
	}
}
