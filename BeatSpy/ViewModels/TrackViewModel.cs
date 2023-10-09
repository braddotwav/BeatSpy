using BeatSpy.Models;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class TrackViewModel : ObservableObject
{
    private BeatTrack? track;
    public BeatTrack? Track
    {
        get { return track; }
        set
        {
            track = value;
            OnPropertyChanged(nameof(Track));
            OnPropertyChanged(nameof(IsTrackEmpty));
        }
    }

    public bool IsTrackEmpty => track == null;
}