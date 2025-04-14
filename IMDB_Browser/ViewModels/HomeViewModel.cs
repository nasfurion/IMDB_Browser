using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagingAndData.Commands;

namespace IMDB_Browser.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged, ISearchable
    {
        private const int PageSize = 100; // Number of items per page
        private readonly INavigationService _navigationService;
        private readonly FavouritesViewModel _favouritesViewModel;
        private readonly WatchListViewModel _watchListViewModel;
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Title> _titles;
        private ObservableCollection<Title> _filteredTitles;
        private string _searchQuery;
        private int _currentPage;
        private int _totalPages;

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
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));

                    CurrentPage = 1;
                    OnPropertyChanged(nameof(CurrentPage));

                    // Filter titles whenever the search query changes
                    _ = FilterTitles();  // Trigger filtering async
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(PageInfo));
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(PageInfo));
            }
        }

        public string PageInfo => $"Page {CurrentPage} of {TotalPages}";

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ToggleWatchlistCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        // Default constructor
        public HomeViewModel()
        {
            _filteredTitles = new ObservableCollection<Title>();
            _titles = new ObservableCollection<Title>();
            _currentPage = 1;

            ToggleFavoriteCommand = new RelayCommand(param => ToggleFavorite((Title)param));
            ToggleWatchlistCommand = new RelayCommand(param => ToggleWatchlist((Title)param));
            NextPageCommand = new RelayCommand(_ => NextPage(), _ => CanGoToNextPage());
            PreviousPageCommand = new RelayCommand(_ => PreviousPage(), _ => CanGoToPreviousPage());
        }

        // Constructor with INavigationService, FavouritesViewModel, and WatchListViewModel
        public HomeViewModel(INavigationService navigationService, FavouritesViewModel favouritesViewModel, WatchListViewModel watchListViewModel, MainViewModel mainViewModel) : this()
        {
            _navigationService = navigationService;
            _favouritesViewModel = favouritesViewModel;
            _watchListViewModel = watchListViewModel;
            _mainViewModel = mainViewModel;
        }

        public async Task FilterTitles()
        {
            if (_titles == null || !_titles.Any())
            {
                Console.WriteLine("Titles collection is empty. No filtering will be performed.");
                return;
            }

            IEnumerable<Title> filtered;

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = _titles
                    .Where(title => title.TitleType == "movie")
                    .OrderByDescending(title => title.StartYear);
            }
            else
            {
                filtered = _titles
                    .Where(title => title.PrimaryTitle != null &&
                                    title.PrimaryTitle.StartsWith(SearchQuery, StringComparison.OrdinalIgnoreCase) &&
                                    title.TitleType == "movie")
                    .OrderByDescending(title => title.StartYear);
            }

            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)filtered.Count() / PageSize);

            // Get the titles for the current page
            var pagedTitles = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            // Update FilteredTitles in-place for better UI performance
            _filteredTitles.Clear();
            foreach (var item in pagedTitles)
                _filteredTitles.Add(item);

            Console.WriteLine($"FilteredTitles updated with {FilteredTitles.Count} items.");

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
                int currentPage = 1; // Start at page 1
                bool foundPoster = false;

                // Loop through all pages until we find a matching poster or exhaust the pages
                while (!foundPoster)
                {
                    string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key=bf901c064acd8676c98fc66dc8efb60f&query={query}&language=en-US&page={currentPage}";

                    try
                    {
                        var response = await client.GetStringAsync(apiUrl);
                        var jsonResponse = JsonConvert.DeserializeObject<JObject>(response);
                        var results = jsonResponse["results"] as JArray;
                        var totalPages = jsonResponse["total_pages"]?.ToObject<int>() ?? 1;

                        if (results != null && results.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                var releaseDate = result["release_date"]?.ToString();
                                var posterPath = result["poster_path"]?.ToString();
                                var description = result["overview"]?.ToString();

                                if (!string.IsNullOrEmpty(releaseDate) && DateTime.TryParse(releaseDate, out var releaseDateParsed))
                                {
                                    if (title.StartYear.HasValue && releaseDateParsed.Year == title.StartYear.Value)
                                    {
                                        title.Description = !string.IsNullOrEmpty(description) ? description : "No description available.";
                                        title.PosterPath = !string.IsNullOrEmpty(posterPath)
                                            ? $"https://image.tmdb.org/t/p/w500{posterPath}"
                                            : "/Assets/IMDB-placeholder.png";
                                        foundPoster = true;
                                        break;
                                    }
                                }
                            }
                        }

                        // If we have found the poster, exit the loop
                        if (foundPoster) break;

                        // If we haven't found the poster, check if we are on the last page
                        if (currentPage >= totalPages)
                        {
                            break; // Exit the loop if we've reached the last page
                        }

                        // Otherwise, move to the next page
                        currentPage++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching poster for {title.PrimaryTitle}: {ex.Message}");
                        title.PosterPath = "/Assets/IMDB-placeholder.png";
                        break;
                    }
                }

                // If no poster is found, set a placeholder
                if (!foundPoster)
                {
                    title.PosterPath = "/Assets/IMDB-placeholder.png";
                }
            }
        }

        private void ToggleFavorite(Title title)
        {
            if (title != null)
            {
                title.IsFavorite = !title.IsFavorite;
                OnPropertyChanged(nameof(Titles));
                _favouritesViewModel.UpdateFavorites(title);
            }
        }

        private void ToggleWatchlist(Title title)
        {
            if (title != null)
            {
                title.IsInWatchlist = !title.IsInWatchlist;
                OnPropertyChanged(nameof(Titles));
                _watchListViewModel.UpdateWatchList(title);
            }
        }

        private void NextPage()
        {
            if (CanGoToNextPage())
            {
                CurrentPage++;
                _ = FilterTitles();
            }
        }

        private void PreviousPage()
        {
            if (CanGoToPreviousPage())
            {
                CurrentPage--;
                _ = FilterTitles();
            }
        }

        private bool CanGoToNextPage() => CurrentPage < TotalPages;

        private bool CanGoToPreviousPage() => CurrentPage > 1;

        public ICommand NavigateToMediaDetailsCommand => _mainViewModel.NavigateToMediaDetailsCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

