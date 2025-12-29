using SmithereenUWP.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SmithereenUWP.Extensions
{
    internal class UIExt
    {
        public static readonly DependencyProperty LocKeyProperty =
            DependencyProperty.RegisterAttached("LocKey", typeof(string), typeof(UIExt), new PropertyMetadata(default));

        public static readonly DependencyProperty PlaceholderLocKeyProperty =
            DependencyProperty.RegisterAttached("PlaceholderLocKey", typeof(string), typeof(UIExt), new PropertyMetadata(default));

        public static void SetLocKey(ButtonBase element, string value)
        {
            element.SetValue(LocKeyProperty, value);
            SetFromLoc(element);
        }

        public static string GetLocKey(ButtonBase element)
        {
            return (string)element.GetValue(LocKeyProperty);
        }

        public static void SetPlaceholderLocKey(TextBox element, string value)
        {
            element.SetValue(LocKeyProperty, value);
            SetPlaceholderFromLoc(element);
        }

        public static string GetPlaceholderLocKey(TextBox element)
        {
            return (string)element.GetValue(LocKeyProperty);
        }

        //

        private static void SetFromLoc(ButtonBase btn)
        {
            string key = GetLocKey(btn);
            if (key == null)
            {
                btn.Content = null;
                return;
            }

            btn.Content = Locale.Get(key);
        }

        private static void SetPlaceholderFromLoc(TextBox tb)
        {
            string key = GetPlaceholderLocKey(tb);
            if (key == null)
            {
                tb.PlaceholderText = null;
                return;
            }

            tb.PlaceholderText = Locale.Get(key);
        }
    }
}
