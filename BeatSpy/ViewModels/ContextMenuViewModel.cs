using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class ContextMenuViewModel : ObservableObject
{
    private bool isConnected = false;
    public bool IsConnected 
    {
        get => isConnected;
        set
        {
            isConnected = value;
            OnPropertyChanged(nameof(IsConnected));
        }
    }

    public ICommand LoginSpotify { get; }
    public ICommand LogOutSpotify { get; }

    public ContextMenuViewModel(ISpotifyService service)
    {
        LoginSpotify = new LogInToSpotifyCommand(service);
        LogOutSpotify = new LogOutOfSpotifyCommand(service);
    }
}