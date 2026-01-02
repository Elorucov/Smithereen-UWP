using SmithereenUWP.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Extensions
{
    internal static class RichTextBlockExt
    {
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached(
                "Html",
                typeof(string),
                typeof(RichTextBlockExt),
                new PropertyMetadata(null, HtmlChanged));

        public static void SetHtml(DependencyObject obj, string value)
            => obj.SetValue(HtmlProperty, value);

        public static string GetHtml(DependencyObject obj)
            => (string)obj.GetValue(HtmlProperty);

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var richText = d as RichTextBlock;
            if (richText == null || e.NewValue == null) return;

            var xhtml = e.NewValue as string;
            HTMLTextParser.Parse(xhtml, richText);
        }
    }
}
