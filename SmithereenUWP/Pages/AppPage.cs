using SmithereenUWP.ViewModels.Base;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Pages
{
    public class AppPage : Page
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(object), typeof(AppPage), new PropertyMetadata(default));

        public static readonly DependencyProperty HeaderAfterContentProperty = DependencyProperty.Register(
            nameof(HeaderAfterContent), typeof(object), typeof(AppPage), new PropertyMetadata(default));

        public static readonly DependencyProperty TopPaddingProperty = DependencyProperty.Register(
            nameof(TopPadding), typeof(double), typeof(AppPage), new PropertyMetadata(0));

        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public object HeaderAfterContent
        {
            get { return GetValue(HeaderAfterContentProperty); }
            set { SetValue(HeaderAfterContentProperty, value); }
        }

        public double TopPadding
        {
            get { return (double)GetValue(TopPaddingProperty); }
            set { SetValue(TopPaddingProperty, value); }
        }
    }

    public class AppPage<T> : AppPage where T : BaseViewModel
    {
        public T ViewModel => DataContext as T;
        public AppPage()
        {
            DataContext = Activator.CreateInstance<T>();
        }
    }
}
