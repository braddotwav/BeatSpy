using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace BeatSpy.DataTypes.Interfaces;

public interface ISpotifyService
{
    SpotifyClient? Client { get; }
    Task Connect();
    bool IsConnected();
}
