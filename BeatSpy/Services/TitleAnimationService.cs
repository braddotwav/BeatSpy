using System;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BeatSpy.Services;

internal class TitleAnimationService : ITitleAnimationService
{
    private readonly DoubleAnimation animation;
    public bool IsPlaying { get; private set; }

    private readonly double containerWidth;

    public TitleAnimationService()
    {
        // Create a double animation
        animation = new()
        {
            EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut},
            AutoReverse = true,
        };

        // Subscribe to the animation completed event
        animation.Completed += (_, _) =>
        {
            IsPlaying = false;
        };
    }

    public void PlayAnimation(TranslateTransform transform, double width)
    {
        animation.To = -(width - 312);
        animation.Duration = TimeSpan.FromSeconds(width / 110);

        transform.BeginAnimation(TranslateTransform.XProperty, animation);
        IsPlaying = true;
    }

    public void StopAnimation(TranslateTransform transform)
    {
        transform.BeginAnimation(TranslateTransform.XProperty, null);
        IsPlaying = false;
    }

    /// <summary>
    /// Determines if the title width is considered to be out of view
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public bool IsTitleOutOfView(double width)
    {
        return width > containerWidth;
    }
}
