using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IMDB_Browser.Models;
using IMDB_Browser.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB_Browser.Views
{
    /// <summary>
    /// Interaction logic for FavouritesViews.xaml
    /// </summary>
    public partial class FavouritesViews : Page
    {
        public FavouritesViews()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).ServiceProvider.GetService<FavouritesViewModel>(); // defining the DataContext for the FavouritesViews
        }

        private void PosterImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                // Handle poster image click
                MessageBox.Show($"Poster clicked: {title.PrimaryTitle}");
            }
        }

        private void ToggleWatchlist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                // Handle toggle watchlist click
                title.IsInWatchlist = !title.IsInWatchlist;
            }
        }

        private void ToggleFavorite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                // Handle toggle favorite click
                title.IsFavorite = !title.IsFavorite;
            }
        }
    }
}
