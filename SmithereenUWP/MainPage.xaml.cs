using SmithereenUWP.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmithereenUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        SmithereenAPI _api;

        private void CheckServer(object sender, RoutedEventArgs e)
        {
            new Action(async () => {
                _api = new SmithereenAPI(ServerUrl.Text, "ELOR's Test Client 1.0");
                var info = await _api.Server.GetInfoAsync();
                Test.Text = $"Domain: {info.Domain}\n\nName: {info.Name}\n\nDescription: {info.Description}";
            })();
        }
    }
}
