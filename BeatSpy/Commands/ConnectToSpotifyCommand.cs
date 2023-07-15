using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Commands;

internal class ConnectToSpotifyCommand : AsyncCommandBase
{
    private readonly ISpotifyService service;

    public ConnectToSpotifyCommand(ISpotifyService service)
    {
        this.service = service;
    }

    public override bool CanExecute(object? parameter)
    {
        return service is not null;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        await Task.Run(() => service.Connect());
    }
}
