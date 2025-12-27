using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Controls
{
    internal class FixedFontIcon : FontIcon
    {
        public FixedFontIcon()
        {
            SetValue(FontFamilyProperty, App.Current.Resources["SymbolThemeFontFamily"]);
            SetValue(FontSizeProperty, 16);
        }
    }
}
