using System.ComponentModel;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;

namespace IMDB_Browser.ViewModels
{
    public class MediaDetailViewModel : INotifyPropertyChanged, IParameterReceiver
    {
        private Title _selectedTitle;

        public Title SelectedTitle
        {
            get => _selectedTitle;
            set
            {
                _selectedTitle = value;
                OnPropertyChanged(nameof(SelectedTitle));
            }
        }

        public void ReceiveParameter(object parameter)
        {
            if (parameter is Title title)
            {
                SelectedTitle = title;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
