using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class ListenOnSpotifyCommand : CommandBase
{
    private readonly ISpotifyService spotifyService;

    public ListenOnSpotifyCommand(ISpotifyService spotifyService)
    {
        this.spotifyService = spotifyService;
    }

    public override void Execute(object? parameter)
    {
        if(parameter is BeatTrack track)
        {
            BrowserHelper.OpenURLInBrowser(track.TrackUrl!);
        }
    }
}
