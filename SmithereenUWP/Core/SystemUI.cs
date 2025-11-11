using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SmithereenUWP.Core
{
    public static class SystemUI
    {
        private static Dictionary<int, Popup> _windowCustomTitleBars = new Dictionary<int, Popup>();

        public static void ExtendView()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            var tb = ApplicationView.GetForCurrentView().TitleBar;
            tb.ButtonBackgroundColor = Colors.Transparent;
            tb.ButtonInactiveBackgroundColor = Colors.Transparent;

            if (Functions.IsDesktop) SetupCustomTitleBar();
        }

        private static void SetupCustomTitleBar()
        {
            var id = ApplicationView.GetForCurrentView().Id;
            if (!_windowCustomTitleBars.ContainsKey(id))
            {
                Popup p = CreateCustomTitleBarPopup();
                p.IsOpen = true;
                _windowCustomTitleBars.Add(id, p);
            }
            else
            {
                _windowCustomTitleBars[id].IsOpen = true;
            }
        }

        private static Popup CreateCustomTitleBarPopup()
        {
            var sysTB = CoreApplication.GetCurrentView().TitleBar;
            var view = CoreApplication.GetCurrentView();

            ContentPresenter b = new ContentPresenter
            {
                Background = new SolidColorBrush(Colors.Transparent),
                ContentTemplate = (DataTemplate)App.Current.Resources["TitlebarContent"],
            };

            var popup = new Popup
            {
                Child = b,
            };

            FixSize(b, popup, sysTB, Window.Current.Bounds.Width);

            sysTB.LayoutMetricsChanged += async (x, y) =>
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => FixSize(b, popup, x, Window.Current.Bounds.Width));
            };
            Window.Current.SizeChanged += async (x, y) =>
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var tb = CoreApplication.GetCurrentView().TitleBar;
                    FixSize(b, popup, tb, Window.Current.Bounds.Width);
                });
            };
            Window.Current.SetTitleBar(b);

            return popup;
        }

        private static void FixSize(FrameworkElement c, Popup p, CoreApplicationViewTitleBar tb, double width)
        {
            var cw = width - tb.SystemOverlayLeftInset - tb.SystemOverlayRightInset;
            var pw = width - tb.SystemOverlayRightInset;
            c.Margin = new Thickness(tb.SystemOverlayLeftInset, 0, 0, 0);
            c.Width = cw; c.Height = tb.Height;
            p.Width = pw; c.Height = tb.Height;
        }
    }
}
