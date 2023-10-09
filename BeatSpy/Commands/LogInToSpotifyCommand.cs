using BeatSpy.Services;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;

namespace BeatSpy.Commands;

internal class LogInToSpotifyCommand : AsyncCommandBase
{
    private readonly ISpotifyService service;

    public LogInToSpotifyCommand(ISpotifyService service)
    {
        this.service = service;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        await Task.Run(() => service.Login());
    }
}
