using SmithereenUWP.Core;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Extensions
{
    internal static class TextBlockExt
    {
        public static readonly DependencyProperty LocKeyProperty =
            DependencyProperty.RegisterAttached("LocKey", typeof(string), typeof(TextBlockExt), new PropertyMetadata(default));

        public static void SetLocKey(TextBlock element, string value)
        {
            element.SetValue(LocKeyProperty, value);
            SetFromLoc(element);
        }

        public static string GetLocKey(TextBlock element)
        {
            return (string)element.GetValue(LocKeyProperty);
        }


        #region Internal

        static Dictionary<TextBlock, long> _registered = new Dictionary<TextBlock, long>();

        private static void SetFromLoc(TextBlock tb)
        {
            string key = GetLocKey(tb);
            if (key == null)
            {
                tb.Text = string.Empty;
                return;
            }

            tb.Text = Locale.Get(key);
        }

        #endregion
    }
}
