using System;
using SpotifyAPI.Web;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Services;

public interface ISpotifyService
{
    /// <summary>
    /// Is the user currently logged in
    /// </summary>
    public bool IsLoggedIn { get; }

    /// <summary>
    /// Asynchronously logs in the user based on login type
    /// </summary>
    /// <param name="loginType"></param>
    /// <returns></returns>
    public Task LoginAsync(LoginType loginType);

    /// <summary>
    /// Logs out the user
    /// </summary>
    public void LogOut();

    /// <summary>
    /// An event that is raised when the service state changes
    /// </summary>
    public event Action<ConnectionType> OnServiceStateChanged;

    /// <summary>
    /// Asynchronously returns a random track from a playlist
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns></returns>
    public Task<FullTrack> GetRandomTrackFromPlaylistAsync(string playlistId);

    /// <summary>
    /// Asynchronously returns the first track from a search query
    /// </summary>
    /// <param name="searchQuery">Search query</param>
    /// <returns></returns>
    public Task<FullTrack> GetTrackAsync(string searchQuery);

    /// <summary>
    /// Asynchronously returns the audio features of a track
    /// </summary>
    /// <param name="trackId">Track ID</param>
    /// <returns></returns>
    public Task<TrackAudioFeatures> GetAudioTrackFeaturesAsync(string trackId);
}
