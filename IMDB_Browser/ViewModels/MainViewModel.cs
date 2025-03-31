using IMDB_Browser.Navigation;
using PagingAndData.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMDB_Browser.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        // navigation commands
        private readonly INavigationService _navigationService;
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            CurrentViewModel = new HomeViewModel();
        }

        // Commands for navigation
        public ICommand NavigateToHomeCommand => new RelayCommand(_ => _navigationService.NavigateTo<HomeViewModel>());
        public ICommand NavigateToFavouritesCommand => new RelayCommand(_ => _navigationService.NavigateTo<FavouritesViewModel>());
        public ICommand NavigateToMediaDetailsCommand => new RelayCommand(_ => _navigationService.NavigateTo<MediaDetailViewModel>());
        public ICommand NavigateToWatchListCommand => new RelayCommand(_ => _navigationService.NavigateTo<WatchListViewModel>());
        public ICommand GoBackCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
