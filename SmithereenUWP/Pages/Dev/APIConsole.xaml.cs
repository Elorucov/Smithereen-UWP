using Newtonsoft.Json.Linq;
using SmithereenUWP.API;
using SmithereenUWP.Controls.Popups;
using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using SmithereenUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Dev
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class APIConsole : Page
    {
        private readonly ObservableCollection<KeyValueViewModel> _parameters = new ObservableCollection<KeyValueViewModel>();
        private readonly SmithereenAPI _api;

        public APIConsole()
        {
            this.InitializeComponent();
            DataContext = this;
            if (SessionViewModel.Current?.API != null)
            {
                _api = SessionViewModel.Current.API;
            }
            else
            {
                _api = new SmithereenAPI(AppParameters.CurrentServer, AppInfo.UserAgent)
                {
                    AccessToken = AppParameters.CurrentUserAccessToken
                };
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ParametersList.ItemsSource = _parameters;

            MethodName.Text = "users.get";
            _parameters.Add(new KeyValueViewModel("user_ids", "grishka,oru"));
            _parameters.Add(new KeyValueViewModel("fields", "domain,sex"));
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= e.NewSize.Height)
            {
                Grid.SetColumn(RequestPanel, 0);
                Grid.SetColumnSpan(RequestPanel, 1);
                Grid.SetRow(RequestPanel, 0);
                Grid.SetRowSpan(RequestPanel, 2);
                Grid.SetColumn(ResponsePanel, 1);
                Grid.SetColumnSpan(ResponsePanel, 1);
                Grid.SetRow(ResponsePanel, 0);
                Grid.SetRowSpan(ResponsePanel, 2);
            }
            else
            {
                Grid.SetColumn(RequestPanel, 0);
                Grid.SetColumnSpan(RequestPanel, 2);
                Grid.SetRow(RequestPanel, 0);
                Grid.SetRowSpan(RequestPanel, 1);
                Grid.SetColumn(ResponsePanel, 0);
                Grid.SetColumnSpan(ResponsePanel, 2);
                Grid.SetRow(ResponsePanel, 1);
                Grid.SetRowSpan(ResponsePanel, 1);
            }
        }

        private void ClearParameters(object sender, RoutedEventArgs e)
        {
            _parameters.Clear();
        }

        private void AddParameter(object sender, RoutedEventArgs e)
        {
            _parameters.Add(new KeyValueViewModel());
        }

        private void SendRequest(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MethodName.Text)) return;

            new Action(async () =>
            {
                ScreenSpinner<string> ssp = new ScreenSpinner<string>();

                try
                {
                    Dictionary<string, string> parameters = _parameters.Where(kv => !string.IsNullOrEmpty(kv.Key))
                        .ToDictionary(kv => kv.Key, kv => kv.Value);

                    var response = await ssp.ShowAsync(_api.SendRequestAsync(MethodName.Text, parameters));
                    JToken parsed = JToken.Parse(response);
                    ResponseBox.Text = parsed.ToString(Newtonsoft.Json.Formatting.Indented);
                }
                catch (SmithereenAPIException apiex)
                {
                    ResponseBox.Text = $"API Error {apiex.Code}:\n{apiex.Message}";
                }
                catch (Exception ex)
                {
                    ResponseBox.Text = $"Error {ex.ToHEX()}:\n{ex.Message}";
                }
            })();
        }

        private void RemoveParameter(object sender, RoutedEventArgs e)
        {
            KeyValueViewModel parameter = ((FrameworkElement)sender).DataContext as KeyValueViewModel;
            _parameters.Remove(parameter);
        }

        private void ExecuteScript(object sender, RoutedEventArgs e)
        {
            new Action(async() => {
                ScreenSpinner<string> ssp = new ScreenSpinner<string>();
                try
                {
                    Dictionary<string, string> parameters = _parameters.Where(kv => !string.IsNullOrEmpty(kv.Key) && kv.Key != "code")
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                    parameters.Add("code", Script.Text);

                    var response = await ssp.ShowAsync(_api.SendRequestAsync("execute", parameters));
                    JToken parsed = JToken.Parse(response);
                    ResponseBox.Text = parsed.ToString(Newtonsoft.Json.Formatting.Indented);
                }
                catch (SmithereenAPIException apiex)
                {
                    ResponseBox.Text = $"API Error {apiex.Code}:\n{apiex.Message}";
                }
                catch (Exception ex)
                {
                    ResponseBox.Text = $"Error {ex.ToHEX()}:\n{ex.Message}";
                }
            })();
        }
    }
}
