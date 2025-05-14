using System;

namespace Inventory.Extensions
{
    public class UppercaseConverter : CommonValueConveter
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            return value?.ToString().ToUpper();
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}