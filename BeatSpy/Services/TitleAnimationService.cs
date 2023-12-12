using System;
using System.Windows.Media;
using BeatSpy.DataTypes.Structs;
using System.Windows.Media.Animation;

namespace BeatSpy.Services;

internal class TitleAnimationService : ITitleAnimationService
{
    private readonly DoubleAnimation animation;

    public bool IsPlaying { get; private set; }

    public TitleAnimationService(TitleAnimationInfo animationInfo)
    {
        //Create double animation
        animation = new()
        {
            Duration = TimeSpan.FromSeconds(animationInfo.Duration),
            DecelerationRatio = animationInfo.Deceleration,
            AutoReverse = animationInfo.ShouldReverse,
        };

        //Set the easing function on the animation
        QuadraticEase easeIn = new()
        {
            EasingMode = EasingMode.EaseIn
        };
        animation.EasingFunction = easeIn;

        //Subscribe to the animation completed event
        animation.Completed += (s, _) =>
        {
            IsPlaying = false;
        };
    }

    /// <summary>
    /// Adjust the animation's target position 
    /// </summary>
    /// <param name="position">Destonation position</param>
    public void SetAnimationPosition(double position)
    {
        animation.To = position;
    }

    /// <summary>
    /// Starts a animation using the provided transform
    /// </summary>
    /// <param name="transform">Transform to animate</param>
    public void StartAnimation(TranslateTransform transform)
    {
        transform.BeginAnimation(TranslateTransform.XProperty, animation);
        IsPlaying = true;
    }

    /// <summary>
    /// Determines if the animation should occur based on if the title width exceeds a specified size
    /// And if the animation is already playing
    /// </summary>
    /// <param name="titleWidth">The actual size of the element</param>
    /// <param name="rectSize">The visiable size of element</param>
    /// <returns></returns>
    public bool ShouldAnimate(double titleWidth, float rectSize)
    {
        return titleWidth > rectSize && !IsPlaying;
    }
}
