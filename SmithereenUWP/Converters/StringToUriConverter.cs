using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmithereenUWP.Converters
{
    public sealed class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string str && Uri.IsWellFormedUriString(str, UriKind.Absolute))
            {
                return new Uri(str);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
