using Microsoft.QueryStringDotNET;
using SmithereenUWP.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Core
{
    public static class Functions
    {
        public static bool IsDesktop => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";
        public static bool IsMobile => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

        public static async Task<bool> LaunchOAuthAsync(string serverDomain)
        {
            string scopes = string.Join(" ", Constants.OAuthScopes);
            return await Launcher.LaunchUriAsync(new Uri($"https://{serverDomain}/oauth/authorize?client_id={Constants.OAUTH_CLIENT_ID}&response_type=token&redirect_uri={Constants.OAUTH_REDIRECT_URI}&scope={scopes}"));
        }

        public static void HandleLaunch(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol && args is ProtocolActivatedEventArgs protArgs)
            {
                if (!protArgs.Uri.AbsoluteUri.StartsWith(Constants.OAUTH_REDIRECT_URI) || protArgs.Uri.Fragment.Length <= 1) return;

                string fragment = protArgs.Uri.Fragment.Substring(1);
                var queries = QueryString.Parse(fragment);
                if (queries.Contains("user_id") && queries.Contains("access_token") && int.TryParse(queries["user_id"], out int uid))
                {
                    AppParameters.CurrentUserId = uid;
                    AppParameters.CurrentUserAccessToken = queries["access_token"];

                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                }
            }
        }
    }
}
