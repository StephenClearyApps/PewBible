using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Text;

namespace PewBibleKjv
{
    [Activity(Label = "Choose Chapter")]
    public class ChooseChapterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var bookIndex = Intent.GetIntExtra("BookIndex", -1);
            if (bookIndex == -1)
            {
                StartActivity(typeof(MainActivity));
                return;
            }

            // Set our view from our layout resource
            SetContentView(Resource.Layout.ChooseChapter);

            var heading = FindViewById<TextView>(Resource.Id.chooseChapterHeading);
            heading.Text = Structure.Books[bookIndex].Name;
        }
    }
}