using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IMDB_Browser.Converters
{
    public class WatchlistImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isInWatchlist)
            {
                var imagePath = isInWatchlist ? "/Assets/watchlist-remove.png" : "/Assets/watchlist-add.png";
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
