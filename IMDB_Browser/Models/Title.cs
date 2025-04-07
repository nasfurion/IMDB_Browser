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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
