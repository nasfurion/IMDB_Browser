using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IMDB_Browser.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IMDB_Browser.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged, ISearchable
    {
        private ObservableCollection<Title> _titles;
        private ObservableCollection<Title> _filteredTitles;
        private string _searchQuery;

        public ObservableCollection<Title> Titles
        {
            get => _titles;
            set
            {
                _titles = value;
                OnPropertyChanged(nameof(Titles));
                _ = FilterTitles();  // Fire and forget, it's async now
            }
        }

        public ObservableCollection<Title> FilteredTitles
        {
            get => _filteredTitles;
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
                _ = FilterTitles();  // Fire and forget, it's async now
            }
        }

        public HomeViewModel()
        {
            _filteredTitles = new ObservableCollection<Title>();
        }

        // Now async
        private async Task FilterTitles()
        {
            if (_titles == null) return;

            IEnumerable<Title> filtered;

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = _titles
                    .Where(title => title.TitleType == "movie")
                    .OrderByDescending(title => title.StartYear)
                    .Take(100);
            }
            else
            {
                filtered = _titles
                    .Where(title => title.PrimaryTitle != null &&
                                    title.PrimaryTitle.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) &&
                                    title.TitleType == "movie")
                    .OrderByDescending(title => title.StartYear)
                    .Take(100);
            }

            // Update FilteredTitles in-place for better UI performance
            _filteredTitles.Clear();
            foreach (var item in filtered)
                _filteredTitles.Add(item);

            // Fetch poster paths for each filtered title
            await FetchPostersForTitles();
        }

        private async Task FetchPostersForTitles()
        {
            if (_filteredTitles == null || !_filteredTitles.Any()) return;

            using HttpClient client = new HttpClient();

            foreach (var title in _filteredTitles)
            {
                string query = Uri.EscapeDataString(title.PrimaryTitle ?? "");
                string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key=bf901c064acd8676c98fc66dc8efb60f&query={query}&language=en-US";

                try
                {
                    var response = await client.GetStringAsync(apiUrl);
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(response);
                    var results = jsonResponse["results"] as JArray;

                    if (results != null && results.Count > 0)
                    {
                        var firstResult = results[0];
                        var posterPath = firstResult["poster_path"]?.ToString();

                        title.PosterPath = !string.IsNullOrEmpty(posterPath)
                            ? $"https://image.tmdb.org/t/p/w500{posterPath}"
                            : "/Assets/IMDB-placeholder.png";
                    }
                    else
                    {
                        title.PosterPath = "/Assets/IMDB-placeholder.png";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching poster for {title.PrimaryTitle}: {ex.Message}");
                    title.PosterPath = "/Assets/IMDB-placeholder.png";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
