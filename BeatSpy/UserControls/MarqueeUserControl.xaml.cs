using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace BeatSpy.UserControls
{
    /// <summary>
    /// Interaction logic for MarqueeUserControl.xaml
    /// </summary>
    public partial class MarqueeUserControl : UserControl
    {
        public static readonly DependencyProperty MarqueePaddingProperty = DependencyProperty.Register("MarqueePadding", typeof(double), typeof(MarqueeUserControl), new PropertyMetadata(10.0));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MarqueeUserControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ScrollSpeedProperty = DependencyProperty.Register("ScrollSpeed", typeof(double), typeof(MarqueeUserControl), new PropertyMetadata(100.0));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double ScrollSpeed
        {
            get { return (double)GetValue(ScrollSpeedProperty); }
            set { SetValue (ScrollSpeedProperty, value); }
        }

        public double MarqueePadding
        {
            get { return (double)GetValue(MarqueePaddingProperty); }
            set { SetValue(MarqueePaddingProperty, value); }
        }

        private bool isMarqueePlaying = false;
        private const int CONTAINER_WIDTH = 312;
        private const int WAIT_TIME = 1000;
        private readonly DoubleAnimation marqueeAnimation;
        private readonly TranslateTransform translateTransform;

        public MarqueeUserControl()
        {
            InitializeComponent();

            // Set up the marquee animation with a simple easing function and auto reverse enabled
            marqueeAnimation = new()
            {
                EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut },
                AutoReverse = true,
            };

            translateTransform = new TranslateTransform();
            MarqueeText.RenderTransform = translateTransform;
            
            marqueeAnimation.Completed += MarqueeAnimation_Completed;

            MarqueeText.MouseEnter += MarqueeText_MouseEnter;
            MarqueeText.TargetUpdated += MarqueeText_TargetUpdated;
        }

        private void MarqueeAnimation_Completed(object? sender, System.EventArgs e)
        {
            isMarqueePlaying = false;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            marqueeAnimation.Completed -= MarqueeAnimation_Completed;

            MarqueeText.MouseEnter -= MarqueeText_MouseEnter;
            MarqueeText.TargetUpdated -= MarqueeText_TargetUpdated;
        }

        private async void MarqueeText_TargetUpdated(object? sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (isMarqueePlaying)
                StopAnimation();

            await Task.Delay(WAIT_TIME);

            if(!isMarqueePlaying && IsTextOutOfBounds())
            {
                PlayAnimation();
            }
        }

        private async void MarqueeText_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isMarqueePlaying)
                return;

            await Task.Delay(WAIT_TIME);

            if (MarqueeText.IsMouseOver && IsTextOutOfBounds())
            {
                PlayAnimation();
            }
        }

        private void StopAnimation()
        {
            translateTransform.BeginAnimation(TranslateTransform.XProperty, null);
            isMarqueePlaying = false;
        }

        private void PlayAnimation()
        {
            marqueeAnimation.To = -(MarqueeText.ActualWidth - (CONTAINER_WIDTH - MarqueePadding));
            marqueeAnimation.Duration = TimeSpan.FromSeconds(MarqueeText.ActualWidth / ScrollSpeed);

            translateTransform.BeginAnimation(TranslateTransform.XProperty, marqueeAnimation);
            isMarqueePlaying = true;
        }

        private bool IsTextOutOfBounds()
        {
            return MarqueeText.ActualWidth > CONTAINER_WIDTH;
        }
    }
}
