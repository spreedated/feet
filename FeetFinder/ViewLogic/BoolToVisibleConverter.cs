using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Globalization;
using System.Windows;

namespace FeetFinder.ViewLogic
{
    public class BoolToVisibleConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = (bool)value;

            return v ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
