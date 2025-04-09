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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
