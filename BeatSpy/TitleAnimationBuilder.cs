using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BeatSpy
{
    internal class TitleAnimationBuilder
    {
        private readonly DoubleAnimation animation;
        public bool IsAnimationPlaying { get; private set; }

        public TitleAnimationBuilder(Duration animationDuration, double deceleration)
        {
            animation = new()
            {
                Duration = animationDuration,
                DecelerationRatio = deceleration,
                AutoReverse = true,
            };

            QuadraticEase easeIn = new()
            {
                EasingMode = EasingMode.EaseIn
            };

            animation.EasingFunction = easeIn;

            animation.Completed += (s, _) =>
            {
                IsAnimationPlaying = false;
            };
        }

        public void StartAnimation(TranslateTransform transform) 
        {
            transform.BeginAnimation(TranslateTransform.XProperty, animation);
            IsAnimationPlaying = true;
        }

        public bool ShouldAnimate(double width)
        {
            return width > 290;
        }

        public void SetAnimationTo(double widthX)
        {
            animation.To = widthX;
        }
    }
}
