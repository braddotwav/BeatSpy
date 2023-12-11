using System;
using SpotifyAPI.Web;
using BeatSpy.DataTypes.Enums;
using System.Threading.Tasks;

namespace BeatSpy.Services;

public interface ISpotifyService
{
    public SpotifyClient? Client { get; }
    public bool IsLoggedIn { get; }
    public event Action<ConnectionType> OnServiceStateChanged;
    public Task<FullTrack> GetRandomTrackFromPlaylist(string playlistId);
    public Task<FullTrack> GetTrack(string query);
    public Task<TrackAudioFeatures> GetAudioTrackFeatures(string trackId);
    public Task LogIn(LoginType loginType);
    public void LogOut();
}
