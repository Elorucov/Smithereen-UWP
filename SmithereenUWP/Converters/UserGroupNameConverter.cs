using SmithereenUWP.API.Objects.Main;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmithereenUWP.Converters
{
    public sealed class UserGroupNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is User user)
            {
                if (parameter != null)
                {
                    return string.Join(" ", user.FirstName, user.Nickname, user.LastName, user.MaidenName);
                }
                else
                {
                    return string.Join(" ", user.FirstName, user.LastName);
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
