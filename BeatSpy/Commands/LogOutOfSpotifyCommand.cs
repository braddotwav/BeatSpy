using BeatSpy.Commands.Base;
using BeatSpy.Services;

namespace BeatSpy.Commands;

internal class LogOutOfSpotifyCommand : CommandBase
{
    private readonly ISpotifyService spotifyService;

    public LogOutOfSpotifyCommand(ISpotifyService spotifyService)
    {
        this.spotifyService = spotifyService;
    }

    public override void Execute(object? parameter)
    {
        spotifyService.LogOut();
    }
}