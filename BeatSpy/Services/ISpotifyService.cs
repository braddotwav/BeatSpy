using System;
using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace BeatSpy.Services;

public interface ISpotifyService
{
    public SpotifyClient? Client { get; }
    public event Action OnConnected;
    public event Action OnDisconnected;
    public Task Connect();
    public Task Login();
    public void Disconnect();
    public bool IsConnected();
}
