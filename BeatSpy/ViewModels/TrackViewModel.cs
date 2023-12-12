using BeatSpy.Models;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class TrackViewModel : ViewModelBase
{
    public BeatTrack? Track { get; private set; }

    public bool IsTrackEmpty => Track == null;

    /// <summary>
    /// Set the current track and update property notifications
    /// </summary>
    /// <param name="track"></param>
    public void SetCurrentTrack(BeatTrack track)
    {
        Track = track;
        OnPropertyChanged(nameof(Track));
        OnPropertyChanged(nameof(IsTrackEmpty));
    }
}