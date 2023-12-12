using System.Windows.Media;

namespace BeatSpy.Services;

public interface ITitleAnimationService
{
    public bool IsPlaying { get; }
    public void StartAnimation(TranslateTransform transform);
    public bool ShouldAnimate(double titleWidth, float rectSize);
    public void SetAnimationPosition(double position);
}