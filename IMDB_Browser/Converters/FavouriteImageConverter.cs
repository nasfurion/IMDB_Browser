using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IMDB_Browser.Converters
{
    public class FavoriteImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorite)
            {
                var imagePath = isFavorite ? "/Assets/favourite-remove.png" : "/Assets/favourite-add.png";
                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
