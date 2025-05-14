using System;
#if WINDOWS_UWP
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif

namespace Inventory.Extensions
{

    public abstract class CommonValueConveter : IValueConverter
    {
        protected abstract object Convert(object value, Type targetType, object parameter);
        protected abstract object ConvertBack(object value, Type targetType, object parameter);

#if WINDOWS_UWP
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ConvertBack(value, targetType, parameter);
        }
#else
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter);
        }
#endif
    }
}
