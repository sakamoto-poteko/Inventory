using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Inventory.Extensions
{
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool x)
            {
                return !x;
            }
            else
            {
                throw new ArgumentException("value must be bool");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool x)
            {
                return !x;
            }
            else
            {
                throw new ArgumentException("value must be bool");
            }
        }
    }
}
