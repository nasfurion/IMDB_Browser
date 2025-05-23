﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;
using IMDB_Browser.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB_Browser.Views
{
    /// <summary>
    /// Interaction logic for WatchListView.xaml
    /// </summary>
    public partial class WatchListView : Page
    {
        public WatchListView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).ServiceProvider.GetService<WatchListViewModel>(); // defining the DataContext for the WatchlistView
        }

        private void PosterImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                // Handle poster image click
                var navigationService = ((App)Application.Current).ServiceProvider.GetService<INavigationService>();
                navigationService?.NavigateTo<MediaDetailViewModel>(title);
            }
        }

        private void ToggleWatchlist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                var homeViewModel = ((App)Application.Current).ServiceProvider.GetService<HomeViewModel>();
                homeViewModel.ToggleWatchlistCommand.Execute(title);
            }
        }

        private void ToggleFavorite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is Title title)
            {
                var homeViewModel = ((App)Application.Current).ServiceProvider.GetService<HomeViewModel>();
                homeViewModel.ToggleFavoriteCommand.Execute(title);
            }
        }
    }
}
