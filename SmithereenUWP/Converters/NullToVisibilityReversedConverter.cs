using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmithereenUWP.Converters
{
    public sealed class NullToVisibilityReversedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return Visibility.Visible;
            if (value is bool b && !b) return Visibility.Visible;
            if (value is string s && string.IsNullOrEmpty(s)) return Visibility.Visible;
            if (value is int i && i == 0) return Visibility.Visible;
            if (value is ICollection collection && collection.Count == 0) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
