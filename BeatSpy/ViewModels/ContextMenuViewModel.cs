using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class ContextMenuViewModel : ViewModelBase
{
    public ICommand LoginSpotify { get; }
    public ICommand LogOutSpotify { get; }

    public ContextMenuViewModel(ISpotifyService service, IMessageDisplayService messageDisplayService)
    {
        LoginSpotify = new LogInToSpotifyCommand(service, messageDisplayService);
        LogOutSpotify = new LogOutOfSpotifyCommand(service);
    }
}