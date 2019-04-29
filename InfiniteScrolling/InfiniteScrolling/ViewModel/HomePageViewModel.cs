using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using InfiniteScrolling.Model;
using Xamarin.Forms.Extended;

namespace InfiniteScrolling.ViewModel
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isBusy;
        private string _search;
        private readonly int _pageSize = 25;       
        private readonly DataService _dataService;
        private bool _showLoadingText;

        #endregion

        public HomePageViewModel()
        {
            _dataService = new DataService();
            Task.Run(async () =>
            {
                Items = new InfiniteScrollCollection<string>
                {
                    OnLoadMore = async () =>
                    {
                        var page = Items.Count / _pageSize;
                        var items = await Get(Search, page);
                        return items;
                    },
                    OnCanLoadMore = () =>
                    {
                        ShowLoadingText = (Items.Count < TotalCount);
                        return ShowLoadingText;
                    },
                };
                await Items.LoadMoreAsync();
            });
        }

        public async Task Filter()
        {
            Items.Clear();
            await Items.LoadMoreAsync();
        }

        public async Task<IEnumerable<string>> Get(string searchTxt, int page)
        {
            (int count, var dataset) = await _dataService.GetAll(searchTxt, page, _pageSize);
            TotalCount = count;
            return dataset;
        }

        #region Getters and Setters

        public InfiniteScrollCollection<string> Items { get; private set; }

        public int TotalCount { get; private set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public string Search
        {
            get { return _search; }
            set
            {
                if (_search == value)
                    return;

                _search = value;
                string searchText = _search;
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    if (_search == searchText)
                    {
                        await Filter();
                    }
                });
                OnPropertyChanged(nameof(Search));
            }
        }

        public bool ShowLoadingText
        {
            get { return _showLoadingText; }
            set
            {
                if (_showLoadingText == value)
                    return;

                _showLoadingText = value;
                OnPropertyChanged(nameof(ShowLoadingText));
            }
        }

        #endregion

        #region Property change implementation

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Property change implementation
    }
}
