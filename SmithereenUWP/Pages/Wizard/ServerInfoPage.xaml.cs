using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Wizard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServerInfoPage : Page
    {
        private ServerInfo _serverInfo;

        public ServerInfoPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _serverInfo = e.Parameter as ServerInfo;
            ServerName.Text = _serverInfo.Name;
            Domain.Text = _serverInfo.Domain;
            ShortDesc.Text = _serverInfo.ShortDescription;
        }

        private void OpenExternalAuth(object sender, RoutedEventArgs e)
        {
            new Action(async () => await Functions.LaunchOAuthAsync(_serverInfo.Domain))();
        }
    }
}
