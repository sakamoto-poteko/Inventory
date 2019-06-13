using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Inventory.Extensions
{
    public class InvertedBoolConverter : CommonValueConveter
    {
        protected override object Convert(object value, Type targetType, object parameter)
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

        protected override object ConvertBack(object value, Type targetType, object parameter)
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
