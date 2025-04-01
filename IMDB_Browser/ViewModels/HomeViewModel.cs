using System.ComponentModel;

namespace IMDB_Browser.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged, ISearchable
    {
        private string _searchQuery;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));

                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            // Add logic to filter/search based on _searchQuery
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
