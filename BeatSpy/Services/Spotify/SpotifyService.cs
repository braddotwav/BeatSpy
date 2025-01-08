using System;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Services.Spotify;

internal sealed class SpotifyService(ISpotifyAuthenticationService authenticationService) : ISpotifyService
{
    private readonly ISpotifyAuthenticationService authenticationService = authenticationService;

    private SpotifyClient? client;

    public bool IsLoggedIn => client != null;

    public event Action<ConnectionType>? OnServiceStateChanged;

    public async Task LoginAsync(LoginType loginType)
    {
        switch (loginType)
        {
            case LoginType.Automatic:
                client = await authenticationService.ConnectAsync();
                OnServiceStateChanged?.Invoke(client == null ? ConnectionType.Disconnected : ConnectionType.Connected);
                break;
            case LoginType.Manual:
                await authenticationService.LoginAsync();
                goto case LoginType.Automatic;
        }
    }

    public void LogOut()
    {
        client = null;
        OnServiceStateChanged?.Invoke(client != null ? ConnectionType.Connected : ConnectionType.Disconnected);
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

        FullTrack track = (FullTrack)playlist.Tracks!.Items![RandomHelper.Range(0, playlist.Tracks.Items.Count)].Track;
        return track;
    }
}