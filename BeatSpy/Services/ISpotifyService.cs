using System;
using SpotifyAPI.Web;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Services;

public interface ISpotifyService
{
    /// <summary>
    /// Spotify client
    /// </summary>
    public SpotifyClient? Client { get; }

    /// <summary>
    /// Is the user currently logged in
    /// </summary>
    public bool IsLoggedIn { get; }

    /// <summary>
    /// This event is fired whenever the connection state changes
    /// </summary>
    public event Action<ConnectionType> OnServiceStateChanged;

    /// <summary>
    /// Asynchronously returns a random track from a playlist
    /// </summary>
    /// <param name="playlistID">Playlist ID</param>
    /// <returns></returns>
    public Task<FullTrack> GetRandomTrackFromPlaylistAsync(string playlistID);

    /// <summary>
    /// Asynchronously returns the first track from a search query
    /// </summary>
    /// <param name="searchQuery">Search query</param>
    /// <returns></returns>
    public Task<FullTrack> GetTrackAsync(string searchQuery);

    /// <summary>
    /// Asynchronously returns the audio features of a track
    /// </summary>
    /// <param name="trackID">Track ID</param>
    /// <returns></returns>
    public Task<TrackAudioFeatures> GetAudioTrackFeaturesAsync(string trackID);

    /// <summary>
    /// Asynchronously logs in
    /// </summary>
    /// <param name="loginType"></param>
    /// <returns></returns>
    public Task LoginAsync(LoginType loginType);

    /// <summary>
    /// Nulls the spotify client and logs the user out
    /// </summary>
    public void LogOut();
}
