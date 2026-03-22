using SmithereenUWP.Extensions;
using SmithereenUWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Profile
{
    public class ProfilePageBase : AppPage { }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : ProfilePageBase
    {
        public ProfileViewModel ViewModel => DataContext as ProfileViewModel;

        public ProfilePage()
        {
            this.InitializeComponent();
            RegisterPropertyChangedCallback(TopPaddingProperty, TopPaddingChanged);
        }

        private void TopPaddingChanged(DependencyObject sender, DependencyProperty dp)
        {
            ContentRootWrap.Padding = new Thickness(0, TopPadding, 0, 0);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is int id)
            {
                DataContext = new ProfileViewModel(id);
            } else
            {
                DataContext = new ProfileViewModel();
            }
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

        private void DataContextForNarrowPhotoAlbumChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Image image = sender as Image;
            if (ViewModel?.PhotoAlbums != null && ViewModel.PhotoAlbums.Count > 0)
            {
                ImageExt.SetPhotoSource(image, ViewModel.PhotoAlbums[0].Cover);
            }
            else
            {
                image.Source = null;
            }
        }

        private void ShowExtraInfosWide(object sender, RoutedEventArgs e)
        {
            WideShowFullInfoArea.Visibility = Visibility.Collapsed;
            WideExtraInfos.Visibility = Visibility.Visible;
        }

        private void ShowExtraInfosNarrow(object sender, RoutedEventArgs e)
        {
            (sender as FrameworkElement).Visibility = Visibility.Collapsed;
            NarrowWideExtraInfos.Visibility = Visibility.Visible;
        }
    }
}
