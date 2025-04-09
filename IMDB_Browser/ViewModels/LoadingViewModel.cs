using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using IMDB_Browser.Navigation;

namespace IMDB_Browser.ViewModels
{
    public class LoadingViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        public LoadingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoadMovies();
        }

        private async void LoadMovies()
        {
            // Navigate to HomeViewModel after loading
            _navigationService.NavigateTo<HomeViewModel>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
