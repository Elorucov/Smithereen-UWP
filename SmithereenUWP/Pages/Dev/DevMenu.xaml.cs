using SmithereenUWP.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages.Dev
{
    public class DebugMenuItem
    {
        public string Name { get; private set; }
        public Type Page { get; private set; }
        public Action Action { get; set; }

        public DebugMenuItem(string name, Type page, Action action = null)
        {
            Name = name; Page = page; Action = action;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevMenu : Page
    {
        private ApplicationView AppView => ApplicationView.GetForCurrentView();
        private CoreApplicationView CoreAppView => CoreApplication.GetCurrentView();

        public DevMenu()
        {
            this.InitializeComponent();
            uid.Text = $"Server: {AppParameters.CurrentServer}, user ID: {AppParameters.CurrentUserId}";

            AppView.VisibleBoundsChanged += AppView_VisibleBoundsChanged;
            CoreAppView.TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
            Loaded += DevMenu_Loaded;
            Unloaded += DevMenu_Unloaded;

            List<DebugMenuItem> items = new List<DebugMenuItem> {
                new DebugMenuItem("Dev settings", typeof(DevSettings))
            };
            Menu.ItemsSource = items;
        }

        private void DevMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            AppView.VisibleBoundsChanged -= AppView_VisibleBoundsChanged;
            CoreAppView.TitleBar.LayoutMetricsChanged -= TitleBar_LayoutMetricsChanged;
            Loaded -= DevMenu_Loaded;
            Unloaded -= DevMenu_Unloaded;
        }

        private void DevMenu_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager navmgr = SystemNavigationManager.GetForCurrentView();
            navmgr.BackRequested += (a, b) => {
                if (Frame.CanGoBack)
                {
                    b.Handled = true;
                    Frame.GoBack(new DrillInNavigationTransitionInfo());
                }
            };
            Frame.Navigated += (a, b) => {
                navmgr.AppViewBackButtonVisibility = Frame.BackStackDepth <= 0 ? AppViewBackButtonVisibility.Collapsed : AppViewBackButtonVisibility.Visible;
            };
        }

        private void AppView_VisibleBoundsChanged(ApplicationView sender, object args)
        {
            SetupInsets();
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            SetupInsets();
        }

        private void SetupInsets()
        {
            var vb = AppView.VisibleBounds;
            var ws = Window.Current.Bounds;
            double titleBar = Functions.IsDesktop ? CoreAppView.TitleBar.Height : 0;
            double l = !Functions.IsDesktop ? vb.Left : 0;
            double r = !Functions.IsDesktop ? ws.Width - vb.Right : 0;
            double t = !Functions.IsDesktop ? vb.Top : titleBar;
            double b = !Functions.IsDesktop ? ws.Height - vb.Bottom : 0;

            (Window.Current.Content as Frame).Padding = new Thickness(l, t, Math.Max(0, r), Math.Max(0, b));
        }

        private void Menu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is DebugMenuItem)
            {
                DebugMenuItem item = e.ClickedItem as DebugMenuItem;
                if (item.Action == null)
                {
                    Frame.Navigate(item.Page, null, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    item.Action.Invoke();
                }
            }
        }
    }
}
