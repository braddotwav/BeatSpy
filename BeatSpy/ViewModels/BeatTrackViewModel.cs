using BeatSpy.Models;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class BeatTrackViewModel : ObservableObject
{
    private BeatTrack? currentTrack;
    public BeatTrack? CurrentTrack
    {
        get { return currentTrack; }
        set 
        { 
            currentTrack = value; 
            OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(IsTrackEmpty));
        }
    }

    public bool IsTrackEmpty => string.IsNullOrEmpty(currentTrack?.TrackTitle);
}