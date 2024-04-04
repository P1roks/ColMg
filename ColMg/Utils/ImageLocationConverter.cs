using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.Utils
{
    internal class ImageLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string stringValue)
            {
                return CollectionRepository.getImagePath(stringValue);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string stringValue)
            {
                return Path.GetFileName(stringValue);
            }
            return 0;
        }
    }
}
