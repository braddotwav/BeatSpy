using NLog;
using System;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace BeatSpy.Services;

internal class SpotifyService : ISpotifyService, IDisposable
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private const string CLIENT_ID = "4b675b489cf74ff2a4a2d4483cf1dbe1";
    private const string REDIRECT_URI = "http://127.0.0.1:5543/callback";
    
    public bool IsConnected => client != null;

    private SpotifyClient? client;
    private readonly OAuthAuthenticator authenticator;

    public event Action<bool>? OnConnectionStateChanged;

    public SpotifyService()
    {
        authenticator = new(CLIENT_ID, new Uri(REDIRECT_URI));
    }

    public async Task<bool> AuthenticateAsync(CancellationToken cancellationToken)
    {
        try
        {
            Task authTask = authenticator.Authenticate();
            Task cancelTask = Task.Delay(Timeout.Infinite, cancellationToken);

            Task completedTask = await Task.WhenAny(authTask, cancelTask);

            if (completedTask == cancelTask)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return false;
            }

            await authTask;
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            throw;
        }

        return true;
    }

    public async Task<bool> ConnectAsync()
    {
        try
        {
            client = await authenticator.ConnectAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            return false;
        }

        OnConnectionStateChanged?.Invoke(IsConnected);
        return true;
    }

    public async Task<Track> GetTrackFromSearchAsync(string query)
    {
        try
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("The search query cannot be null or empty.", nameof(query));

            if (client == null)
                throw new InvalidOperationException("The client is not initialized.");

            SearchResponse reponse = await client.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));

            if (reponse.Tracks.Items == null)
                throw new InvalidOperationException("Tracks items are null, which is now allowed.");

            FullTrack track = await client.Tracks.Get(reponse.Tracks.Items[0].Id);
            TrackAudioFeatures features = await client.Tracks.GetAudioFeatures(track.Id);

            return new Track(track, features);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            throw;
        }
    }

    public async Task<Track> GetRandomTrackFromPlaylistAsync(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("The id query cannot be empty or have whitespace.", nameof(id));

            if (client == null)
                throw new InvalidOperationException("The client is not initialized.");

            FullPlaylist playlist = await client.Playlists.Get(id);

            if (playlist.Tracks == null || playlist.Tracks.Items == null)
                throw new InvalidOperationException("Playlist does not contain any tracks, which is not allowed.");

            FullTrack track = (FullTrack)playlist.Tracks.Items[RandomHelper.Range(0, playlist.Tracks.Items.Count)].Track;
            TrackAudioFeatures features = await client.Tracks.GetAudioFeatures(track.Id);

            return new Track(track, features);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            throw;
        }
    }

    public void Disconnect()
    {
        client = null;
        authenticator.Disconnect();
        OnConnectionStateChanged?.Invoke(IsConnected);
    }

    public void Dispose()
    {
        authenticator?.Dispose();
    }
}