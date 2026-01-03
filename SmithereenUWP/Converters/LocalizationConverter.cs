using SmithereenUWP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmithereenUWP.Converters
{
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string key)
            {
                return Locale.Get(key);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
