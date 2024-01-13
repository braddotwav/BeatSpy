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

    public async Task LoginAsync(LoginType loginType)
    {
        switch (loginType)
        {
            case LoginType.Automatic:
                Client = await authenticationService.ConnectAsync();
                break;
            case LoginType.Manual:
                await authenticationService.LoginAsync();
                goto case LoginType.Automatic;
        }
    }

    public void LogOut()
    {
        Client = null;
        authenticationService.LogOut();
    }
    
    public async Task<FullTrack> GetTrackAsync(string searchQuery)
    {
        SearchResponse response = await client!.Search.Item(new SearchRequest(SearchRequest.Types.Track, searchQuery!));

        FullTrack track = await client.Tracks.Get(response.Tracks.Items![0].Id);
        return track;
    }

    public async Task<TrackAudioFeatures> GetAudioTrackFeaturesAsync(string trackId)
    {
        TrackAudioFeatures trackFeatures = await client!.Tracks.GetAudioFeatures(trackId);
        return trackFeatures;
    }

    public async Task<FullTrack> GetRandomTrackFromPlaylistAsync(string playlistId)
    {
        FullPlaylist playlist = await client!.Playlists.Get(playlistId);

        if (playlist.Tracks == null)
            throw new NullReferenceException();

        FullTrack track = (FullTrack)playlist.Tracks.Items![RandomRange.Range(0, playlist.Tracks.Items.Count)].Track;
        return track;
    }
}