using SmithereenUWP.ViewModels;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Profile
{
    public class ProfilePageBase : AppPage<ProfileViewModel> { }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : ProfilePageBase
    {
        public ProfilePage()
        {
            this.InitializeComponent();
            RegisterPropertyChangedCallback(TopPaddingProperty, TopPaddingChanged);
        }

        private void TopPaddingChanged(DependencyObject sender, DependencyProperty dp)
        {
            ContentRootWrap.Padding = new Thickness(0, TopPadding, 0, 0);
        }
    }
}
