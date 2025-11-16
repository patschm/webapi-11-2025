using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProductReviews.Client.Wpf.Utils
{
    public class HttpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                value = "laserprinters.png";
            }
            return $"https://angular-training.azureedge.net/{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value!.ToString()!.Replace("https://angular-training.azureedge.net/", "");
        }
    }
}
