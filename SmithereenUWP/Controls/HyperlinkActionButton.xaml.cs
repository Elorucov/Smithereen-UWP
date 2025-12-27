using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SmithereenUWP.Controls
{
    public sealed partial class HyperlinkActionButton : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(HyperlinkActionButton), new PropertyMetadata(default));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register(nameof(Count), typeof(int), typeof(HyperlinkActionButton), new PropertyMetadata(default));

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(HyperlinkActionButton), new PropertyMetadata(default));

        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add { MainButton.Click += value; }
            remove { MainButton.Click -= value; }
        }

        long _lid = 0;
        long _cid = 0;
        long _gid = 0;

        public HyperlinkActionButton()
        {
            this.InitializeComponent();
            DataContext = this;

            _lid = RegisterPropertyChangedCallback(LabelProperty, (a, b) => UpdateUI());
            _cid = RegisterPropertyChangedCallback(CountProperty, (a, b) => UpdateUI());
            _gid = RegisterPropertyChangedCallback(IconGlyphProperty, (a, b) => UpdateUI());


            Unloaded += HyperlinkActionButton_Unloaded;
        }

        private void HyperlinkActionButton_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= HyperlinkActionButton_Unloaded;
            UnregisterPropertyChangedCallback(LabelProperty, _lid);
            UnregisterPropertyChangedCallback(CountProperty, _cid);
            UnregisterPropertyChangedCallback(IconGlyphProperty, _gid);
        }

        private void UpdateUI()
        {
            HABIcon.Visibility = !string.IsNullOrWhiteSpace(IconGlyph) ? Visibility.Visible : Visibility.Collapsed;
            HABLabel.Visibility = !string.IsNullOrWhiteSpace(Label) ? Visibility.Visible : Visibility.Collapsed;
            HABCounter.Visibility = Count != 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
