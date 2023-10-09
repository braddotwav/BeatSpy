using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class ContextMenuViewModel : ObservableObject
{
    private bool isConnectd = false;
    public bool IsConnected 
    {
        get => isConnectd;
        set
        {
            isConnectd = value;
            OnPropertyChanged(nameof(IsConnected));
        }
    }

    public ICommand LoginSpotify { get; }
    public ICommand LogOutSpotify { get; }
    public ICommand ExitApplication { get; }

    public ContextMenuViewModel(ISpotifyService service)
    {
        LoginSpotify = new LogInToSpotifyCommand(service);
        LogOutSpotify = new LogOutOfSpotifyCommand(service);
        ExitApplication = new ExitApplicationCommand();
    }
}