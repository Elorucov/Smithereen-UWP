using SmithereenUWP.API;
using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Controls.Popups;
using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Wizard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            new Action(async () => await CheckServerAsync())();
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                new Action(async () => await CheckServerAsync())();
            }
        }

        private async Task CheckServerAsync()
        {
            string serverUrl = ServerUrlBox.Text;
            if (!Uri.IsWellFormedUriString($"https://{serverUrl}", UriKind.Absolute))
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "Invalid URL",
                    PrimaryButtonText = "Close"
                }.ShowAsync();
                return;
            }

            try
            {
                SmithereenAPI api = new SmithereenAPI(serverUrl, AppInfo.UserAgent);

                ScreenSpinner<ServerInfo> ssp = new ScreenSpinner<ServerInfo>();
                var info = await ssp.ShowAsync(api.Server.GetInfoAsync());

                Frame.Navigate(typeof(ServerInfoPage), info, new SlideNavigationTransitionInfo());

            }
            catch (Exception ex)
            {
                await ex.ShowAsync();
            }
        }
    }
}
