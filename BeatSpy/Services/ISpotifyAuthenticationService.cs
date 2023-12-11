using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace BeatSpy.Services;

public interface ISpotifyAuthenticationService
{
    public Task<SpotifyClient> Connect();
    public Task LogIn();
    public void LogOut();
}