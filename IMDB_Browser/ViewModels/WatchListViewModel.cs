using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using IMDB_Browser.Models;
using PagingAndData.Commands;

namespace IMDB_Browser.ViewModels
{
    public class WatchListViewModel : INotifyPropertyChanged, ISearchable
    {
        private ObservableCollection<Title> _watchListMovies;
        private ObservableCollection<Title> _filteredWatchListMovies;
        private string _searchQuery;

        public ObservableCollection<Title> WatchListMovies
        {
            get => _filteredWatchListMovies;
            set
            {
                _filteredWatchListMovies = value;
                OnPropertyChanged(nameof(WatchListMovies));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterMovies();
            }
        }

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ToggleWatchlistCommand { get; }

        public WatchListViewModel()
        {
            _watchListMovies = new ObservableCollection<Title>();
            _filteredWatchListMovies = new ObservableCollection<Title>(_watchListMovies);
            ToggleFavoriteCommand = new RelayCommand(param => ToggleFavorite((Title)param));
            ToggleWatchlistCommand = new RelayCommand(param => ToggleWatchlist((Title)param));
        }

        public void UpdateWatchList(Title title)
        {
            if (title.IsInWatchlist)
            {
                if (!_watchListMovies.Contains(title))
                {
                    _watchListMovies.Add(title);
                }
            }
            else
            {
                if (_watchListMovies.Contains(title))
                {
                    _watchListMovies.Remove(title);
                }
            }
            FilterMovies();
            OnPropertyChanged(nameof(WatchListMovies));
        }

        private void FilterMovies()
        {
            if (string.IsNullOrWhiteSpace(_searchQuery))
            {
                WatchListMovies = new ObservableCollection<Title>(_watchListMovies);
            }
            else
            {
                WatchListMovies = new ObservableCollection<Title>(_watchListMovies
                    .Where(movie => movie.PrimaryTitle != null && movie.PrimaryTitle.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void ToggleFavorite(Title title)
        {
            if (title != null)
            {
                title.IsFavorite = !title.IsFavorite;
                OnPropertyChanged(nameof(WatchListMovies));
            }
        }

        private void ToggleWatchlist(Title title)
        {
            if (title != null)
            {
                title.IsInWatchlist = !title.IsInWatchlist;
                UpdateWatchList(title);
                OnPropertyChanged(nameof(WatchListMovies));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

