using System;
using BeatSpy.Services;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Commands;

internal sealed class LogInToSpotifyCommand(ISpotifyService spotifyService, IMessageDisplayService messageDisplayService) : AsyncCommandBase
{
    private readonly IMessageDisplayService messageDisplayService = messageDisplayService;
    private readonly ISpotifyService spotifyService = spotifyService;

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            await Task.Run(() => spotifyService.LoginAsync(LoginType.Manual));
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}
