using SpotifyAPI.Web;
using BeatSpy.Helpers;

namespace BeatSpy.Models;

internal class BeatTrack
{ 
    public string? TrackArtist { get; }
    public string? TrackTitle { get; set; }
    public string? TrackCoverUrl { get; }
    public string? TrackUrl { get; }
    public string? TrackVolume { get; }
    public string? TrackTempo { get; }
    public string? TrackKey { get; }
    public string? TrackDuration { get; }

    public BeatTrack(FullTrack SpotifyFullTrack, TrackAudioFeatures SpotifyAudioFeatures)
    {
        TrackArtist = SpotifyFullTrack.Artists[0].Name;
        TrackTitle = SpotifyFullTrack.Name;
        TrackCoverUrl = SpotifyFullTrack.Album.Images[0].Url;
        TrackUrl = SpotifyFullTrack.Uri;
        TrackVolume = TrackInfo.GetVolume(SpotifyAudioFeatures.Loudness);
        TrackTempo = TrackInfo.GetTempo(SpotifyAudioFeatures.Tempo);
        TrackKey = TrackInfo.GetKey(SpotifyAudioFeatures.Key);
        TrackDuration = TrackInfo.GetDuration(SpotifyAudioFeatures.DurationMs);
    }
}