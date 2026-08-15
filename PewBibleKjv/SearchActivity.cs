using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using PewBibleKjv.Text;

namespace PewBibleKjv;

[Activity(Label = "Search")]
public class SearchActivity : AppCompatActivity
{
    private EditText _searchInput = null!;
    private TextView _resultCount = null!;
    private SearchResultsAdapter _adapter = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.Search);

        _searchInput = FindViewById<EditText>(Resource.Id.searchInput)!;
        _resultCount = FindViewById<TextView>(Resource.Id.searchResultCount)!;
        var recyclerView = FindViewById<RecyclerView>(Resource.Id.searchResultsRecyclerView)!;
        var searchButton = FindViewById<Button>(Resource.Id.searchButton)!;

        recyclerView.SetLayoutManager(new LinearLayoutManager(this));
        _adapter = new SearchResultsAdapter(LayoutInflater, OnResultClick);
        recyclerView.SetAdapter(_adapter);

        searchButton.Click += (_, __) => RunSearch();
        _searchInput.EditorAction += (_, e) =>
        {
            if (e.ActionId == ImeAction.Search)
                RunSearch();
        };
    }

    private void RunSearch()
    {
        var query = _searchInput.Text ?? "";
        var results = Concordance.Search(query);

        if (results.Count == 0)
        {
            _resultCount.Visibility = ViewStates.Visible;
            _resultCount.Text = query.Trim().Length == 0 ? "" : "No results found.";
        }
        else
        {
            _resultCount.Visibility = ViewStates.Visible;
            _resultCount.Text = $"{results.Count} verse{(results.Count == 1 ? "" : "s")} found.";
        }

        _adapter.SetResults(results);

        // Hide keyboard after search
        var imm = (InputMethodManager?)GetSystemService(InputMethodService)!;
        imm?.HideSoftInputFromWindow(_searchInput.WindowToken, 0);
    }

    private void OnResultClick(int position)
    {
        var absoluteVerseNumber = _adapter.GetVerseNumber(position);
        var intent = new Intent(this, typeof(MainActivity));
        intent.PutExtra("VerseNumber", absoluteVerseNumber);
        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
        StartActivity(intent);
        Finish();
    }

    private sealed class SearchResultsAdapter : RecyclerView.Adapter
    {
        private readonly LayoutInflater _inflater;
        private readonly Action<int> _onResultClick;
        private IReadOnlyList<int> _results = Array.Empty<int>();

        public SearchResultsAdapter(LayoutInflater inflater, Action<int> onResultClick)
        {
            _inflater = inflater;
            _onResultClick = onResultClick;
        }

        public void SetResults(IReadOnlyList<int> results)
        {
            _results = results;
            NotifyDataSetChanged();
        }

        public int GetVerseNumber(int position) => _results[position];

        public override int ItemCount => _results.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _inflater.Inflate(Resource.Layout.SearchResultItem, parent, false)!;
            return new ResultViewHolder(view, _onResultClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var verseNumber = _results[position];
            var location = Location.Create(verseNumber);
            var text = Bible.FormattedVerse(verseNumber).Text.Trim();
            var reference = $"{location.Book.Name} {location.ChapterNumber}:{location.Verse}";

            var vh = (ResultViewHolder)holder;
            vh.Reference.Text = reference;
            vh.VerseText.Text = text;
        }

        private sealed class ResultViewHolder : RecyclerView.ViewHolder
        {
            public TextView Reference { get; }
            public TextView VerseText { get; }

            public ResultViewHolder(View view, Action<int> onResultClick) : base(view)
            {
                Reference = view.FindViewById<TextView>(Resource.Id.searchResultReference)!;
                VerseText = view.FindViewById<TextView>(Resource.Id.searchResultText)!;
                view.Click += (_, __) => onResultClick(AbsoluteAdapterPosition);
            }
        }
    }
}
