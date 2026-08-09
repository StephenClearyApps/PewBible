using System.Diagnostics;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Views;
using AndroidX.AppCompat.App;
using PewBibleKjv.Text;
using PewBibleKjv.Util;

namespace PewBibleKjv;

[Activity(Label = "Choose Chapter", ConfigurationChanges = ConfigChanges.UiMode)]
public class ChooseChapterActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ThemePreferences.ApplyFromPreferences(this);
        var bookIndex = Intent!.GetIntExtra("BookIndex", -1);
        if (bookIndex == -1)
        {
            StartActivity(MainActivityIntent(this));
            return;
        }

        // Set our view from our layout resource
        SetContentView(Resource.Layout.ChooseChapter);

        var heading = FindViewById<TextView>(Resource.Id.chooseChapterHeading)!;
        heading.Text = Structure.Books[bookIndex].Name;

        var grid = FindViewById<GridView>(Resource.Id.chooseChapterGrid)!;
        var buttonWidth = MaximumButtonWidth();
        grid.SetColumnWidth(buttonWidth);
        grid.Adapter = new ChapterAdapter(this, bookIndex, buttonWidth);
    }

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        ThemePreferences.HandleSystemThemeChanged(this);
        Recreate();
    }

    private static Intent MainActivityIntent(Context context)
    {
        var activity = new Intent(context, typeof(MainActivity));
        activity.SetFlags(ActivityFlags.ClearTop);
        return activity;
    }

    private int MaximumButtonWidth()
    {
        var size = AndroidUtils.MeasureLayout(this, LayoutInflater, Resource.Layout.ChooseChapterButton,
            setupView: view =>
            {
                var button = view.FindViewById<Button>(Resource.Id.chooseChapterButton)!;
                button.Text = (Structure.Books.Max(x => x.Chapters.Length) + 1).ToString();
            });
        return size.Width;
    }

    private sealed class ChapterAdapter : ArrayAdapter<string>
    {
        private readonly Context _context;
        private readonly int _bookIndex;
        private readonly int _buttonWidth;

        public ChapterAdapter(Context context, int bookIndex, int buttonWidth)
            : base(context, Resource.Layout.ChooseChapterButton,
                Structure.Books[bookIndex].Chapters.Select(c => (c.Index + 1).ToString()).ToArray())
        {
            _context = context;
            _bookIndex = bookIndex;
            _buttonWidth = buttonWidth;
        }

        public override View GetView(int position, View? convertView, ViewGroup parent)
        {
            var result = base.GetView(position, convertView, parent);
            var button = result.FindViewById<Button>(Resource.Id.chooseChapterButton)!;
            button.SetMinWidth(_buttonWidth);
            button.Click += (_, __) =>
            {
                var chapterIndex = position;
                var activity = MainActivityIntent(_context);
                activity.PutExtra("BookIndex", _bookIndex);
                activity.PutExtra("ChapterIndex", chapterIndex);
                _context.StartActivity(activity);
            };
            return result;
        }
    }
}