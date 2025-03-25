using System;
using BeatSpy.Models;
using System.Threading;
using System.Threading.Tasks;

namespace BeatSpy.Services;

public interface ISpotifyService
{
    public bool IsConnected { get; }
    public event Action<bool> OnConnectionStateChanged;
    public Task<bool> AuthenticateAsync(CancellationToken cancellationToken);
    public Task<bool> ConnectAsync();
    public void Disconnect();
    public Task<Track> GetTrackFromSearchAsync(string query);
    public Task<Track> GetRandomTrackFromPlaylistAsync(string id);
}