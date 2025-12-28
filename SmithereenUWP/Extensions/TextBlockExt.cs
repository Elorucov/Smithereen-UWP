using SmithereenUWP.Core;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace SmithereenUWP.Extensions
{
    internal static class TextBlockExt
    {
        public static readonly DependencyProperty IsSpecialProperty =
            DependencyProperty.RegisterAttached("IsSpecial", typeof(bool), typeof(TextBlockExt), new PropertyMetadata(false));

        public static void SetIsSpecial(TextBlock element, bool value)
        {
            element.SetValue(IsSpecialProperty, value);
            RegisterAsSpecial(element, value);
        }

        public static bool GetIsSpecial(TextBlock element)
        {
            return (bool)element.GetValue(IsSpecialProperty);
        }

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

        private static void RegisterAsSpecial(TextBlock tb, bool enabled)
        {
            if (!enabled)
            {
                Unregister(_registered, tb, TextBlock.TextProperty);
                tb.Text = tb.Text;
                return;
            }

            Register(_registered, tb, TextBlock.TextProperty, (a, b) => SetFromText(tb));
            SetFromText(tb); // TODO: check last updated property: if last time we setting LocKey property, we need call SetFromLoc instead.
            tb.Unloaded += TextBlock_Unloaded;
        }

        private static void TextBlock_Unloaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tb.Unloaded -= TextBlock_Unloaded;
            Unregister(_registered, tb, TextBlock.TextProperty);
        }

        private static void Register(Dictionary<TextBlock, long> dict, TextBlock tb, DependencyProperty dp, DependencyPropertyChangedCallback callback)
        {
            if (!dict.ContainsKey(tb))
            {
                dict.Add(tb, tb.RegisterPropertyChangedCallback(dp, callback));
            }
            else
            {
                throw new ArgumentException("This instance of TextBox already registered");
            }
        }

        private static void Unregister(Dictionary<TextBlock, long> dict, TextBlock tb, DependencyProperty dp)
        {
            if (dict.ContainsKey(tb))
            {
                long id = dict[tb];
                tb.UnregisterPropertyChangedCallback(dp, id);
            }
            else
            {
                throw new ArgumentException("This instance of TextBox is't registered");
            }
        }

        private static void SetFromText(TextBlock tb)
        {
            ParseAndSetHTML(tb, tb.Text);
        }

        private static void SetFromLoc(TextBlock tb)
        {
            string key = GetLocKey(tb);
            if (key == null)
            {
                SetFromText(tb);
                return;
            }

            if (GetIsSpecial(tb))
            {
                ParseAndSetHTML(tb, Locale.Get(key));
            }
            else
            {
                tb.Text = Locale.Get(key);
            }
        }

        private static void ParseAndSetHTML(TextBlock tb, string text)
        {
            tb.Inlines.Clear();
            tb.Inlines.Add(new Run { Text = "AAA=" });
            tb.Inlines.Add(new Run { Text = text });
        }

        #endregion
    }
}
