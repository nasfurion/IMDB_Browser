using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IMDB_Browser.Data; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using IMDB_Browser.Models;

namespace IMDB_Browser.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged, ISearchable
    {
        private ObservableCollection<Title> _titles;
        private ObservableCollection<Title> _filteredTitles;
        public ObservableCollection<string> UniqueTitleTypes { get; set; } = new ObservableCollection<string>();

        private string _searchQuery;
        private ObservableCollection<string> _imageUrls;
        private Dictionary<string, string> _imageCache;
        private System.Threading.Timer _debounceTimer;

        public ObservableCollection<Title> Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                OnPropertyChanged(nameof(Titles));
                FilteredTitles = new ObservableCollection<Title>(_titles);

                // Update UniqueTitleTypes when Titles are set
                var types = _titles
                    .Select(t => t.TitleType)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .OrderBy(t => t);

                UniqueTitleTypes = new ObservableCollection<string>(types);
                OnPropertyChanged(nameof(UniqueTitleTypes));
            }
        }

        public ObservableCollection<Title> FilteredTitles
        {
            get { return _filteredTitles; }
            set
            {
                _filteredTitles = value;
                OnPropertyChanged(nameof(FilteredTitles));
            }
        }
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));

                // Restart debounce timer
                _debounceTimer.Change(300, Timeout.Infinite);
            }
        }

        public HomeViewModel()
        {
            _imageUrls = new ObservableCollection<string>();
            _imageCache = new Dictionary<string, string>();

            // Initialize debounce timer
            _debounceTimer = new System.Threading.Timer(OnDebounceTimerElapsed, null, Timeout.Infinite, 300); // 300ms delay
        }

        

        public ObservableCollection<string> ImageUrls
        {
            get => _imageUrls;
            set
            {
                _imageUrls = value;
                OnPropertyChanged(nameof(ImageUrls));
            }
        }

        private void OnDebounceTimerElapsed(object state)
        {
            PerformSearch();
        }

        private async void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(_searchQuery))
            {
            }
            else
            {
            }

            await FetchMovieImages();
        }

        private async Task FetchMovieImages()
        {
            //Application.Current.Dispatcher.Invoke(() => _imageUrls.Clear());
            //HashSet<string> addedUrls = new HashSet<string>();
            //using (HttpClient client = new HttpClient())
            //{
            //    foreach (var movie in _filteredItems)
            //    {
            //        try
            //        {
            //            if (_imageCache.TryGetValue(movie, out string cachedImageUrl))
            //            {
            //                Application.Current.Dispatcher.Invoke(() => _imageUrls.Add(cachedImageUrl));
            //            }
            //            else
            //            {
            //                string url = $"https://api.themoviedb.org/3/search/movie?api_key=bf901c064acd8676c98fc66dc8efb60f&query={movie}&language=en-US";
            //                var response = await client.GetStringAsync(url);
            //                var jsonDoc = JsonDocument.Parse(response);
            //                var posterPath = jsonDoc.RootElement.GetProperty("results").EnumerateArray().FirstOrDefault().GetProperty("poster_path").GetString();
            //                if (!string.IsNullOrEmpty(posterPath))
            //                {
            //                    string fullPosterPath = $"https://image.tmdb.org/t/p/w500{posterPath}";
            //                    if (addedUrls.Add(fullPosterPath))
            //                    {
            //                        Application.Current.Dispatcher.Invoke(() => _imageUrls.Add(fullPosterPath));
            //                        _imageCache[movie] = fullPosterPath;
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Log the exception (you can replace this with your logging mechanism)
            //            Console.WriteLine($"Error fetching image for movie '{movie}': {ex.Message}");
            //        }
            //    }
            //}
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

