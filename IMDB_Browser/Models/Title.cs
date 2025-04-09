using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Browser.Models
{
    public partial class Title : INotifyPropertyChanged
    {
        private string? _posterPath = string.Empty;
        private string? _description = string.Empty;
        private bool _isFavorite = false;
        private bool _isInWatchlist = false;

        [NotMapped]
        public string? PosterPath
        {
            get => _posterPath;
            set
            {
                if (_posterPath != value)
                {
                    _posterPath = value;
                    OnPropertyChanged(nameof(PosterPath));
                }
            }
        }

        [NotMapped]
        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        [NotMapped]
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite));
                }
            }
        }

        [NotMapped]
        public bool IsInWatchlist
        {
            get => _isInWatchlist;
            set
            {
                if (_isInWatchlist != value)
                {
                    _isInWatchlist = value;
                    OnPropertyChanged(nameof(IsInWatchlist));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
