using SmithereenUWP.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Dev
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevSettings : Page
    {
        public DevSettings()
        {
            this.InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            p01.IsChecked = AppParameters.WallDebug;
            p01.Click += (a, b) => AppParameters.WallDebug = (bool)(a as CheckBox).IsChecked;
        }
    }
}
