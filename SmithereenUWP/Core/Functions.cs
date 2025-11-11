using Microsoft.QueryStringDotNET;
using SmithereenUWP.Pages;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Core
{
    public static class Functions
    {
        private static string _temporaryAuthDomain = null;
        private static string _temporaryAuthState = null;

        public static bool IsDesktop => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";
        public static bool IsMobile => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

        public static async Task<bool> LaunchOAuthAsync(string serverDomain)
        {
            _temporaryAuthDomain = serverDomain;
            _temporaryAuthState = Guid.NewGuid().ToString();

            string scopes = string.Join(" ", Constants.OAuthScopes);
            return await Launcher.LaunchUriAsync(new Uri($"https://{serverDomain}/oauth/authorize?client_id={Constants.OAUTH_CLIENT_ID}&response_type=token&redirect_uri={Constants.OAUTH_REDIRECT_URI}&scope={scopes}&state={_temporaryAuthState}"));
        }

        public static void HandleLaunch(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol && args is ProtocolActivatedEventArgs protArgs)
            {
                if (!protArgs.Uri.AbsoluteUri.StartsWith(Constants.OAUTH_REDIRECT_URI) || protArgs.Uri.Fragment.Length <= 1) return;

                string fragment = protArgs.Uri.Fragment.Substring(1);
                var queries = QueryString.Parse(fragment);
                if (queries.Contains("state") && queries["state"] == _temporaryAuthState
                    && queries.Contains("user_id") && queries.Contains("access_token") && int.TryParse(queries["user_id"], out int uid))
                {
                    AppParameters.CurrentUserId = uid;
                    AppParameters.CurrentUserAccessToken = queries["access_token"];
                    AppParameters.CurrentServer = _temporaryAuthDomain;

                    _temporaryAuthState = null;
                    _temporaryAuthDomain = null;

                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                }
            }
        }
    }
}
