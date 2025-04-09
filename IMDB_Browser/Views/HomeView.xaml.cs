using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IMDB_Browser.Models;
using IMDB_Browser.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB_Browser.Views
{
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).ServiceProvider.GetService<HomeViewModel>(); // defining the DataContext for the HomeView
        }

        private void PosterImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                var viewModel = DataContext as HomeViewModel;
                viewModel?.NavigateToMediaDetailsCommand.Execute(title);
            }
        }

        private void ToggleWatchlist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                var viewModel = DataContext as HomeViewModel;
                viewModel?.ToggleWatchlistCommand.Execute(title);
            }
        }

        private void ToggleFavorite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                var viewModel = DataContext as HomeViewModel;
                viewModel?.ToggleFavoriteCommand.Execute(title);
            }
        }
    }
}
