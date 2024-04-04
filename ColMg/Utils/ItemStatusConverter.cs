using ColMg.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.Utils
{
    public class ItemStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ItemStatus)
            {
                return value.ToString().SplitCamelCase();
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string stringVal)
            {
                return string.Join("", stringVal.Split(' '));
            }
            return 0;
        }
    }
}
