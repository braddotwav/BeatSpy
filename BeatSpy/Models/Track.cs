using SpotifyAPI.Web;

namespace BeatSpy.Models;

public class Track(FullTrack track, TrackAudioFeatures trackFeatures)
{
    public string Artist { get; } = track.Artists[0].Name;
    public string Title { get; } = track.Name;
    public string Release { get; } = track.Album.ReleaseDate;
    public string CoverUrl { get; } = track.Album.Images[0].Url;
    public string Url { get; } = track.Uri;
    public float Loudness { get; } = trackFeatures.Loudness;
    public float Tempo { get; } = trackFeatures.Tempo;
    public int Key { get; } = trackFeatures.Key;
    public int Duration { get; } = trackFeatures.DurationMs;
}