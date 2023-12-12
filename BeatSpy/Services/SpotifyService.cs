using System;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Services;

internal class SpotifyService : ISpotifyService
{
    private readonly ISpotifyAuthenticationService authenticationService;

    private SpotifyClient? client;

    public SpotifyClient? Client
    {
        get { return client; }
        set
        {
            client = value;
            OnServiceStateChanged?.Invoke(client == null ? ConnectionType.Disconnected : ConnectionType.Connected);
        }
    }

    public bool IsLoggedIn => Client != null;

    public event Action<ConnectionType>? OnServiceStateChanged;

    public SpotifyService(ISpotifyAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    /// <summary>
    /// Logs the user in creating a client
    /// </summary>
    /// <param name="loginType">Login type</param>
    /// <returns></returns>
    public async Task LogIn(LoginType loginType)
    {
        switch (loginType)
        {
            case LoginType.Automatic:
                Client = await authenticationService.Connect();
                break;
            case LoginType.Manual:
                await authenticationService.LogIn();
                goto case LoginType.Automatic;
        }
    }

    /// <summary>
    /// Logs the user out by clearing the client and initiating authentication service logout.
    /// </summary>
    public void LogOut()
    {
        Client = null;
        authenticationService.LogOut();
    }

    /// <summary>
    /// Asynchronously searches for a track using the provided search query.
    /// </summary>
    /// <param name="searchQuery">Search query</param>
    /// <returns></returns>
    public async Task<FullTrack> GetTrack(string searchQuery)
    {
        SearchResponse response = await client!.Search.Item(new SearchRequest(SearchRequest.Types.Track, searchQuery!));

        FullTrack track = await client.Tracks.Get(response.Tracks.Items![0].Id);
        return track;
    }

    /// <summary>
    /// Asynchronously fetches the audio features of a provided track using its ID
    /// </summary>
    /// <param name="trackID">Track ID</param>
    /// <returns></returns>
    public async Task<TrackAudioFeatures> GetAudioTrackFeatures(string trackID)
    {
        TrackAudioFeatures trackFeatures = await client!.Tracks.GetAudioFeatures(trackID);
        return trackFeatures;
    }

    /// <summary>
    /// Asynchronously obtains the details of a specific playlist using the provided playlist ID
    /// </summary>
    /// <param name="playlistId">Playlist ID</param>
    /// <returns></returns>
    public async Task<FullTrack> GetRandomTrackFromPlaylist(string playlistId)
    {
        FullPlaylist playlist = await client!.Playlists.Get(playlistId);

        if (playlist.Tracks == null)
            throw new NullReferenceException();

        FullTrack track = (FullTrack)playlist.Tracks.Items![RandomRange.Range(0, playlist.Tracks.Items.Count)].Track;
        return track;
    }
}