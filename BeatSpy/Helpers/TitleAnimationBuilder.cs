using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BeatSpy.Helpers;

internal class TitleAnimationBuilder
{
    private readonly DoubleAnimation animation;
    public bool IsAnimationPlaying { get; private set; }

    /// <summary>
    /// Create the double animation with an ease in curve
    /// </summary>
    /// <param name="animationDuration"></param>
    /// <param name="deceleration"></param>
    public TitleAnimationBuilder(Duration animationDuration, double deceleration)
    {
        //Init animation
        animation = new()
        {
            Duration = animationDuration,
            DecelerationRatio = deceleration,
            AutoReverse = true,
        };

        //Init and set the easing function on the animation
        QuadraticEase easeIn = new()
        {
            EasingMode = EasingMode.EaseIn
        };
        animation.EasingFunction = easeIn;

        //Subscribe to the animation completed event
        animation.Completed += (s, _) =>
        {
            IsAnimationPlaying = false;
        };
    }

    /// <summary>
    /// Starts the animation on the passed transform
    /// </summary>
    /// <param name="transform"></param>
    public void StartAnimation(TranslateTransform transform)
    {
        transform.BeginAnimation(TranslateTransform.XProperty, animation);
        IsAnimationPlaying = true;
    }

    /// <summary>
    /// Should the animation be able to start
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public bool ShouldAnimate(double width)
    {
        return width > 290;
    }

    /// <summary>
    /// Set the animations to position
    /// </summary>
    /// <param name="widthX"></param>
    public void SetAnimationTo(double widthX)
    {
        animation.To = widthX;
    }
}