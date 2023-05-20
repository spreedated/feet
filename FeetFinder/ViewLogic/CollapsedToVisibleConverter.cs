using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Globalization;
using System.Windows;

namespace FeetFinder.ViewLogic
{
    public class CollapsedToVisibleConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            Visibility v = (Visibility)value;

            if (v == Visibility.Collapsed || v == Visibility.Hidden)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }
    }
}
