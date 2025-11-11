using SmithereenUWP.ViewModels;
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
            Test.Height = TopPadding;
        }
    }
}
