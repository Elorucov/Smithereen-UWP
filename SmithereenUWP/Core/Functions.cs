using Microsoft.QueryStringDotNET;
using SmithereenUWP.Pages;
using SmithereenUWP.Pages.Dev;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
                if (protArgs.Uri.Host == "debug")
                {
                    if (App.Current.Activated)
                    {
                        new Action(async () => {
                            var view = await OpenNewViewAsync(typeof(DevMenu), "Dev menu");
                        })();
                    } else
                    {
                        (Window.Current.Content as Frame).Navigate(typeof(DevMenu));
                    }
                    return;
                }

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

        public static async Task<bool> OpenNewViewAsync(Type pageType, string title, object data = null, bool closeOnMainWindowClosing = false)
        {
            var currentAV = ApplicationView.GetForCurrentView();
            Window newWindow = null;
            var newAV = CoreApplication.CreateNewView();
            bool result = false;
            await newAV.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;

                newAppView.SetPreferredMinSize(new Size(320, 480));

                var frame = new Frame();
                frame.Navigate(pageType, data);
                newWindow.Content = frame;
                newWindow.Activate();

                result = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.Custom, currentAV.Id, ViewSizePreference.Custom);
            });
            if (closeOnMainWindowClosing) currentAV.Consolidated += async (a, b) => {
                await newAV.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { newWindow?.Close(); });
            };
            return result;
        }
    }
}
