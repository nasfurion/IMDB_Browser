using System;
using System.ComponentModel;
using System.Windows.Input;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;
using PagingAndData.Commands;

namespace IMDB_Browser.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private object _currentViewModel;
        private string _searchQuery;
        private bool _isSearchVisible;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));

                // If the new ViewModel supports searching, update its SearchQuery
                if (_currentViewModel is ISearchable searchableViewModel)
                {
                    searchableViewModel.SearchQuery = this.SearchQuery;
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));

                // Update search query for the current ViewModel if it supports searching
                if (CurrentViewModel is ISearchable searchableViewModel)
                {
                    searchableViewModel.SearchQuery = value;
                }
            }
        }

        public bool IsSearchVisible
        {
            get => _isSearchVisible;
            set
            {
                _isSearchVisible = value;
                OnPropertyChanged(nameof(IsSearchVisible));
            }
        }

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ToggleWatchlistCommand { get; }

        public MainViewModel(INavigationService navigationService, FavouritesViewModel favouritesViewModel, WatchListViewModel watchlistViewModel)
        {
            _navigationService = navigationService;

            // Pass the navigation service and favouritesViewModel to HomeViewModel
            var homeViewModel = new HomeViewModel(_navigationService, favouritesViewModel, watchlistViewModel, this);
            CurrentViewModel = homeViewModel;

            // Propagate SearchQuery to HomeViewModel directly
            homeViewModel.SearchQuery = this.SearchQuery;

            ToggleFavoriteCommand = new RelayCommand(param => ToggleFavorite((Title)param));
            ToggleWatchlistCommand = new RelayCommand(param => ToggleWatchlist((Title)param));
        }

        private void ToggleFavorite(Title title)
        {
            if (title != null)
            {
                title.IsFavorite = !title.IsFavorite;
            }
        }

        private void ToggleWatchlist(Title title)
        {
            if (title != null)
            {
                title.IsInWatchlist = !title.IsInWatchlist;
            }
        }


        // Navigation Commands
        public ICommand NavigateToHomeCommand => new RelayCommand(_ => _navigationService.NavigateTo<HomeViewModel>());
        public ICommand NavigateToFavouritesCommand => new RelayCommand(_ => _navigationService.NavigateTo<FavouritesViewModel>());
        public ICommand NavigateToMediaDetailsCommand => new RelayCommand(
            parameter =>
            {
                if (parameter is Title selectedTitle)
                {
                    _navigationService.NavigateTo<MediaDetailViewModel>(selectedTitle);
                }
            });
        public ICommand NavigateToWatchListCommand => new RelayCommand(_ => _navigationService.NavigateTo<WatchListViewModel>());
        public ICommand ToggleSearchCommand => new RelayCommand(_ => IsSearchVisible = !IsSearchVisible);

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
