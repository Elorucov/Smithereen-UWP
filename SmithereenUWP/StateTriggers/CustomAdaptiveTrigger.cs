using Windows.UI.Xaml;

namespace SmithereenUWP.StateTriggers
{
    internal class CustomAdaptiveTrigger : StateTriggerBase
    {
        FrameworkElement _current;

        public static readonly DependencyProperty ReferredToProperty =
            DependencyProperty.Register(nameof(ReferredTo), typeof(FrameworkElement), typeof(CustomAdaptiveTrigger), new PropertyMetadata(default, OnHostElementChanged));

        public FrameworkElement ReferredTo
        {
            get { return (FrameworkElement)GetValue(ReferredToProperty); }
            set { SetValue(ReferredToProperty, value); }
        }

        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(nameof(MinWidth), typeof(double), typeof(CustomAdaptiveTrigger), new PropertyMetadata(default));

        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        private static void OnHostElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAdaptiveTrigger trigger = (CustomAdaptiveTrigger)d;
            trigger.Setup();
        }

        private void Setup()
        {
            if (_current != null)
            {
                _current.SizeChanged -= ReferredElementSizeChanged;
            }
            if (ReferredTo != null)
            {
                _current = ReferredTo;
                ReferredTo.SizeChanged += ReferredElementSizeChanged;
            }
        }

        private void ReferredElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= MinWidth)
            {
                SetActive(true);
            }
            else
            {
                SetActive(false);
            }
        }
    }
}