using System;
using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace BeatSpy.Services;

public interface ISpotifyAuthenticationService : IDisposable
{
    /// <summary>
    /// Deletes users token file
    /// </summary>
    public void LogOut();

    /// <summary>
    /// Asynchronously starts a login request
    /// </summary>
    /// <returns></returns>
    public Task LoginAsync();

    /// <summary>
    /// Asynchronously returns a spotify client
    /// </summary>
    /// <returns></returns>
    public Task<SpotifyClient> ConnectAsync();
}