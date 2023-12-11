using BeatSpy.Commands.Base;
using BeatSpy.DataTypes.Enums;
using BeatSpy.Services;
using System;
using System.Threading.Tasks;

namespace BeatSpy.Commands;

internal class LogInToSpotifyCommand : AsyncCommandBase
{
    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;

    public LogInToSpotifyCommand(ISpotifyService spotifyService, IMessageDisplayService messageDisplayService)
    {
        this.spotifyService = spotifyService;
        this.messageDisplayService = messageDisplayService;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            await Task.Run(() => spotifyService.LogIn(LoginType.Manual));
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}
