using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Globalization;

namespace FeetFinder.ViewLogic
{
    public class ReverseBoolConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = (bool)value;
            return v ^ true;
        }
    }
}
