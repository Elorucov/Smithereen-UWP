using SmithereenUWP.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmithereenUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const double WIDE_MIN_WIDTH = 720;

        private double oldWidth = -1;

        private ApplicationView AppView => ApplicationView.GetForCurrentView();
        private CoreApplicationView CoreAppView => CoreApplication.GetCurrentView();

        public MainPage()
        {
            this.InitializeComponent();
            Unloaded += MainPage_Unloaded;
            SizeChanged += MainPage_SizeChanged;
            AppView.VisibleBoundsChanged += MainPage_VisibleBoundsChanged;
            CoreAppView.TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= MainPage_Unloaded;
            SizeChanged -= MainPage_SizeChanged;
            AppView.VisibleBoundsChanged -= MainPage_VisibleBoundsChanged;
            CoreAppView.TitleBar.LayoutMetricsChanged -= TitleBar_LayoutMetricsChanged;
            Loading -= Page_Loading;
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            SystemUI.ExtendView();
            SetupInsets();
            RefreshLayout();
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetupInsets();
            RefreshLayout();
            oldWidth = ActualWidth;
        }

        private void MainPage_VisibleBoundsChanged(ApplicationView sender, object args)
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

            L.Width = l;
            R.Width = Math.Max(0, r);
            T.Height = t;
            B.Height = Math.Max(0, b);
        }

        private void RefreshLayout()
        {
            if (oldWidth > 0)
            {
                if (oldWidth >= WIDE_MIN_WIDTH && ActualWidth >= WIDE_MIN_WIDTH) return;
                if (oldWidth < WIDE_MIN_WIDTH && ActualWidth < WIDE_MIN_WIDTH) return;
            }

            if (ActualWidth < WIDE_MIN_WIDTH)
            {
                Grid.SetColumn(PageContent, 1);
                Grid.SetColumnSpan(PageContent, 2);
                
                WideMenuContainer.Visibility = Visibility.Collapsed;
                WideMenu.Visibility = Visibility.Collapsed;
                WidePageHeaderBackground.Visibility = Visibility.Collapsed;
                WidePageHeader.Visibility = Visibility.Collapsed;

                ToggleNarrowMenuVisibility(false);
                NarrowPageHeaderBackground.Visibility = Visibility.Visible;
                NarrowPageHeader.Visibility = Visibility.Visible;
            } else
            {
                Grid.SetColumn(PageContent, 2);
                Grid.SetColumnSpan(PageContent, 1);

                ToggleNarrowMenuVisibility(false);
                NarrowPageHeaderBackground.Visibility = Visibility.Collapsed;
                NarrowPageHeader.Visibility = Visibility.Collapsed;

                WideMenuContainer.Visibility = Visibility.Visible;
                WideMenu.Visibility = Visibility.Visible;
                WidePageHeaderBackground.Visibility = Visibility.Visible;
                WidePageHeader.Visibility = Visibility.Visible;
            }
        }

        private void ToggleNarrowMenuVisibility(bool visible)
        {
            NarrowMenuLayer.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            NarrowMenuContainer.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            NarrowMenu.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OpenNarrowMenu(object sender, RoutedEventArgs e)
        {
            ToggleNarrowMenuVisibility(true);
        }

        private void CloseNarrowMenu(object sender, TappedRoutedEventArgs e)
        {
            ToggleNarrowMenuVisibility(false);
        }
    }
}
