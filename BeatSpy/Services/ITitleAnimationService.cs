using System.Windows.Media;

namespace BeatSpy.Services;

public interface ITitleAnimationService
{
    public bool IsPlaying { get; }
    public void PlayAnimation(TranslateTransform transform, double width);
    public void StopAnimation(TranslateTransform transform);
    public bool IsTitleOutOfView(double width);
}