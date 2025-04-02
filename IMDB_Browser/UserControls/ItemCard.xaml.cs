using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PagingAndData.Commands;

namespace IMDB_Browser.UserControls
{
    /// <summary>
    /// Interaction logic for ItemCard.xaml
    /// </summary>
    public partial class ItemCard : UserControl, INotifyPropertyChanged
    {

        private string _imageSource;
        private string _title;
        private string _averageRating;
        private string _startYear;
        private string _endYear;
        private string _numberOfEpisodes;
        private bool _isInWatchlist;

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string AverageRating
        {
            get => _averageRating;
            set
            {
                _averageRating = value;
                OnPropertyChanged(nameof(AverageRating));
            }
        }

        public string StartYear
        {
            get => _startYear;
            set
            {
                _startYear = value;
                OnPropertyChanged(nameof(StartYear));
            }
        }

        public string EndYear
        {
            get => _endYear;
            set
            {
                _endYear = value;
                OnPropertyChanged(nameof(EndYear));
            }
        }

        public string NumberOfEpisodes
        {
            get => _numberOfEpisodes;
            set
            {
                _numberOfEpisodes = value;
                OnPropertyChanged(nameof(NumberOfEpisodes));
            }
        }

        public bool IsInWatchlist
        {
            get => _isInWatchlist;
            set
            {
                _isInWatchlist = value;
                OnPropertyChanged(nameof(IsInWatchlist));
            }
        }

        public ICommand AddToFavoritesCommand { get; }
        public ICommand AddToWatchlistCommand { get; }
        
        private void AddToFavorites()
        {
        }

        private void AddToWatchlist()
        {
        }

        public ItemCard()
        {
            //AddToFavoritesCommand = new RelayCommand(AddToFavorites);
            //AddToWatchlistCommand = new RelayCommand(AddToWatchlist);



        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
