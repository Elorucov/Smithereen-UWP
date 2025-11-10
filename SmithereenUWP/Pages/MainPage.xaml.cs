using SmithereenUWP.Core;
using SmithereenUWP.DataModels;
using SmithereenUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        private AppPage _currentPage;

        private ApplicationView AppView => ApplicationView.GetForCurrentView();
        private CoreApplicationView CoreAppView => CoreApplication.GetCurrentView();
        private SessionViewModel ViewModel => DataContext as SessionViewModel;

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
            Loaded -= Page_Loaded;
            PageContent.Navigated -= PageContent_Navigated;
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            SystemUI.ExtendView();
            SetupInsets();
            RefreshLayout();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new SessionViewModel();
            PageContent.Navigate(ViewModel.SelectedMenuItem.PageType);
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

            UpdateCurrentPageSizeProperties();
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
                Grid.SetColumn(PageHeaderBackground, 0);
                Grid.SetColumnSpan(PageHeaderBackground, 4);
                Grid.SetColumn(PageHeader, 1);
                Grid.SetColumnSpan(PageHeader, 2);

                WideMenuContainer.Visibility = Visibility.Collapsed;
                WideMenu.Visibility = Visibility.Collapsed;
                MenuButton.Visibility = Visibility.Visible;

                ToggleNarrowMenuVisibility(false);
            } else
            {
                Grid.SetColumn(PageContent, 2);
                Grid.SetColumnSpan(PageContent, 1);
                Grid.SetColumn(PageHeaderBackground, 2);
                Grid.SetColumnSpan(PageHeaderBackground, 2);
                Grid.SetColumn(PageHeader, 2);
                Grid.SetColumnSpan(PageHeader, 1);

                ToggleNarrowMenuVisibility(false);

                WideMenuContainer.Visibility = Visibility.Visible;
                WideMenu.Visibility = Visibility.Visible;
                MenuButton.Visibility = Visibility.Collapsed;
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

        private void MainMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            ToggleNarrowMenuVisibility(false);

            MainMenuItem choosed = e.ClickedItem as MainMenuItem;
            ViewModel.SelectedMenuItem = choosed;
            PageContent.Navigate(choosed.PageType);
        }

        private void PageContent_Navigated(object sender, NavigationEventArgs e)
        {
            Frame frame = sender as Frame;
            var page = frame.Content as AppPage;
            if (page == null) return;

            _currentPage = page;
            CheckCurrentPageProperties();
            UpdateCurrentPageSizeProperties();
        }

        private void CheckCurrentPageProperties()
        {
            if (_currentPage == null) return;
            PageHeaderContent.Content = _currentPage.Title;
        }

        private void UpdateCurrentPageSizeProperties()
        {
            if (_currentPage == null) return;
            double topPadding = T.Height + PageHeader.ActualHeight;
            _currentPage.TopPadding = topPadding;
        }
    }
}
