using BeatSpy.DataTypes.Interfaces;
using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace BeatSpy.Auth;

internal class SpotifyClientAuth : ISpotifyService
{
    private SpotifyClient? client;
    public SpotifyClient? Client => client;

    private const string CLIENT_SECRET = "";

    public async Task Connect()
    {
        var clientConfig = SpotifyClientConfig.CreateDefault();

        var request = new ClientCredentialsRequest(Properties.Settings.Default.SpotClientId, CLIENT_SECRET);
        var response = await new OAuthClient(clientConfig).RequestToken(request);

        client = new SpotifyClient(clientConfig.WithToken(response.AccessToken));
    }

    public bool IsConnected()
    {
        throw new System.NotImplementedException();
    }
}
