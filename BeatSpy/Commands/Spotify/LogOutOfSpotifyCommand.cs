using BeatSpy.Commands.Base;
using BeatSpy.Services;

namespace BeatSpy.Commands;

internal sealed class LogOutOfSpotifyCommand(ISpotifyService spotifyService) : CommandBase
{
    private readonly ISpotifyService spotifyService = spotifyService;

    public override void Execute(object? parameter)
    {
        spotifyService.LogOut();
    }
}