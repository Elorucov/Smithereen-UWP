using SmithereenUWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

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

        private void LoadProfile(object sender, RoutedEventArgs e)
        {
            new Action(async () => await ViewModel.RefreshDataAsync())();
        }

        private void DataContextChangedForProfilePic(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Image img = sender as Image;
            if (ViewModel == null)
            {
                img.Source = null;
                return;
            }

            // TODO: In the future, implement a smart link chooser system based on control's size.
            string photoUri = img.Width > 100 ? ViewModel.User.Photo400 : ViewModel.User.Photo200;
            if (!ViewModel.User.HasPhoto || !Uri.IsWellFormedUriString(photoUri, UriKind.Absolute))
            {
                img.Source = null;
                return;
            }

            img.Source = new BitmapImage(new Uri(photoUri));
        }
    }
}
