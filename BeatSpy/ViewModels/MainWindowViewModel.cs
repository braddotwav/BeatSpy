using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    /// <summary>
    /// Commands
    /// </summary>
    public ICommand? ListenOnSpotify { get; private set; }
    public ICommand? SearchQueryEntered { get; private set; }

    private ICommand? RandomTrack { get; }

    /// <summary>
    /// View Models
    /// </summary>
    public TrackViewModel TrackViewModel => trackViewModel;
    public MessageHandlerViewModel MessageHandler => messageViewModel;
    public ContextMenuViewModel ContextMenuViewModel => contextMenuViewModel;

    private readonly TrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;
    private readonly ContextMenuViewModel contextMenuViewModel;

    private readonly ISpotifyService spotify;

    public MainWindowViewModel(ISpotifyService spotifyService)
    {
        spotify = spotifyService;
        spotify.OnConnected += OnSpotifyConnected;
        spotify.OnDisconnected += OnSpotifyDisconnected;
        trackViewModel = new();
        messageViewModel = new();
        contextMenuViewModel = new(spotify);
        ListenOnSpotify = new ListenOnSpotifyCommand();
        SearchQueryEntered = new SearchTrackCommand(trackViewModel, messageViewModel, spotify);
        RandomTrack = new RandomTrackCommand(trackViewModel, spotify);
    }

    /// <summary>
    /// This method is raised when the spotify service is connected
    /// </summary>
    private void OnSpotifyConnected()
    {
        if(spotify.Client != null)
        {
            RandomTrack?.Execute(this);
            contextMenuViewModel.IsConnected = true;
        }
    }

    /// <summary>
    /// This method is raised when the spotify service is disconnected
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    private void OnSpotifyDisconnected()
    {
        contextMenuViewModel.IsConnected = false;
        messageViewModel.SetMessage("Disconnected from spotify.");
    }
}
