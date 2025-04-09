using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using IMDB_Browser.Models;
using PagingAndData.Commands;

namespace IMDB_Browser.ViewModels
{
    public class FavouritesViewModel : INotifyPropertyChanged, ISearchable
    {
        private ObservableCollection<Title> _favMovies;
        private ObservableCollection<Title> _filteredFavMovies;
        private string _searchQuery;

        public ObservableCollection<Title> FavMovies
        {
            get => _filteredFavMovies;
            set
            {
                _filteredFavMovies = value;
                OnPropertyChanged(nameof(FavMovies));
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

        public FavouritesViewModel()
        {
            _favMovies = new ObservableCollection<Title>();
            _filteredFavMovies = new ObservableCollection<Title>(_favMovies);
            ToggleFavoriteCommand = new RelayCommand(param => ToggleFavorite((Title)param));
            ToggleWatchlistCommand = new RelayCommand(param => ToggleWatchlist((Title)param));
        }

        public void UpdateFavorites(Title title)
        {
            if (title.IsFavorite)
            {
                if (!_favMovies.Contains(title))
                {
                    _favMovies.Add(title);
                }
            }
            else
            {
                if (_favMovies.Contains(title))
                {
                    _favMovies.Remove(title);
                }
            }
            FilterMovies();
            OnPropertyChanged(nameof(FavMovies));
        }

        private void FilterMovies()
        {
            if (string.IsNullOrWhiteSpace(_searchQuery))
            {
                FavMovies = new ObservableCollection<Title>(_favMovies);
            }
            else
            {
                FavMovies = new ObservableCollection<Title>(_favMovies
                    .Where(movie => movie.PrimaryTitle != null && movie.PrimaryTitle.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void ToggleFavorite(Title title)
        {
            if (title != null)
            {
                title.IsFavorite = !title.IsFavorite;
                OnPropertyChanged(nameof(FavMovies));
            }
        }

        private void ToggleWatchlist(Title title)
        {
            if (title != null)
            {
                title.IsInWatchlist = !title.IsInWatchlist;
                OnPropertyChanged(nameof(FavMovies));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

