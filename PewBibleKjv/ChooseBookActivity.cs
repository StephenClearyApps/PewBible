﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PewBibleKjv.Text;

namespace PewBibleKjv
{
    [Activity(Label = "Choose Book")]
    public class ChooseBookActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from our layout resource
            SetContentView(Resource.Layout.ChooseBook);

            for (var i = 0; i != Structure.Books.Length; ++i)
            {
                var id = Resources.GetIdentifier("button" + i, "id", PackageName);
                var button = FindViewById<Button>(id);
                if (button != null)
                {
                    button.Tag = i;
                    button.Text = Structure.Books[i].Name;
                    button.Click += (sender, __) =>
                    {
                        var bookIndex = (int)((Button)sender).Tag;
                        var activity = new Intent(this, typeof(ChooseChapterActivity));
                        activity.PutExtra("BookIndex", bookIndex);
                        StartActivity(activity);
                    };
                }
            }
        }
    }
}