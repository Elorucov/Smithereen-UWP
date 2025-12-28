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
            SectionsNarrow.DataContext = ViewModel;
            RegisterPropertyChangedCallback(TopPaddingProperty, TopPaddingChanged);
        }

        private void TopPaddingChanged(DependencyObject sender, DependencyProperty dp)
        {
            PostsListTopPadding.Height = TopPadding;
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
