using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IMDB_Browser.Data;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;
using Microsoft.EntityFrameworkCore;

namespace IMDB_Browser.ViewModels
{
    public class MediaDetailViewModel : INotifyPropertyChanged, IParameterReceiver
    {
        private readonly ImdbContext _dbContext;
        private Title _selectedTitle;
        private ObservableCollection<Genre> _genres;

        public Title SelectedTitle
        {
            get => _selectedTitle;
            set
            {
                _selectedTitle = value;
                OnPropertyChanged(nameof(SelectedTitle));
                _ = LoadGenresAsync(); // Load genres when the selected title changes
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged(nameof(Genres));
                OnPropertyChanged(nameof(GenresString));
            }
        }

        public string GenresString => string.Join(", ", Genres.Select(g => g.Name));

        public MediaDetailViewModel(ImdbContext dbContext)
        {
            _dbContext = dbContext;
            _genres = new ObservableCollection<Genre>();
        }

        public void ReceiveParameter(object parameter)
        {
            if (parameter is Title title)
            {
                SelectedTitle = title;
            }
        }

        private async Task LoadGenresAsync()
        {
            if (SelectedTitle != null)
            {
                var genres = await _dbContext.Genres
                    .Where(g => g.Titles.Any(t => t.TitleId == SelectedTitle.TitleId))
                    .ToListAsync();

                Genres = new ObservableCollection<Genre>(genres);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
