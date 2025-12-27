using SmithereenUWP.ViewModels;
using System;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.News
{
    public class NewsPageBase : AppPage<NewsViewModel> { }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsPage : NewsPageBase
    {
        public NewsPage()
        {
            InitializeComponent();

            RegisterPropertyChangedCallback(TopPaddingProperty, TopPaddingChanged);
        }

        private void TopPaddingChanged(DependencyObject sender, DependencyProperty dp)
        {
            PostsListTopPadding.Height = TopPadding;
            SectionsWide.Margin = new Thickness(0, TopPadding, 0, 0);
        }

        private void NewsPageBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= 960)
            {
                SectionsNarrow.Visibility = Visibility.Collapsed;
                SectionsWide.Visibility = Visibility.Visible;
            }
            else
            {
                SectionsWide.Visibility = Visibility.Collapsed;
                SectionsNarrow.Visibility = Visibility.Visible;
            }
        }

        private void NewsPageBase_Loaded(object sender, RoutedEventArgs e)
        {
            new Action(async () =>
            {
                await ViewModel.LoadNewsAsync();
            })();
        }
    }
}
