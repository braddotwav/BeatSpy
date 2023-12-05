using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    //Commands
    public ICommand? ExitApplication { get; }
    public ICommand? MinimizeApplication { get; }
    public ICommand? RemoveFocus { get; }

    public ICommand? ListenOnSpotify { get; }
    public ICommand? SearchQueryEntered { get; }
    private ICommand? RandomTrack { get; }

    //Viewmodels
    public TrackViewModel TrackViewModel => trackViewModel;
    public MessageHandlerViewModel MessageHandler => messageViewModel;
    public ContextMenuViewModel ContextMenuViewModel => contextMenuViewModel;

    private readonly TrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;
    private readonly ContextMenuViewModel contextMenuViewModel;

    //Services
    private readonly ISpotifyService spotify;

    public MainWindowViewModel(ISpotifyService spotifyService)
    {
        spotify = spotifyService;
        spotify.OnConnected += OnSpotifyConnected;
        spotify.OnDisconnected += OnSpotifyDisconnected;
        spotify.OnServiceError += OnSpotifyServiceError;
        trackViewModel = new();
        messageViewModel = new();
        contextMenuViewModel = new(spotify);
        ExitApplication = new ExitApplicationCommand();
        MinimizeApplication = new MinimizeApplicationCommand();
        ListenOnSpotify = new ListenOnSpotifyCommand();
        RemoveFocus = new RemoveFocusCommand();
        SearchQueryEntered = new SearchTrackCommand(this, trackViewModel, messageViewModel, spotify);
        RandomTrack = new RandomTrackCommand(trackViewModel, spotify);
    }

    /// <summary>
    /// This method is raised when the spotify service is connected
    /// </summary>
    private void OnSpotifyConnected()
    {
        if(spotify.IsConnected())
        {
            RandomTrack?.Execute(this);
            contextMenuViewModel.IsConnected = true;
            messageViewModel.ClearMessage();
        }
    }

    /// <summary>
    /// This method is raised when the spotify service is disconnected
    /// </summary>
    private void OnSpotifyDisconnected()
    {
        contextMenuViewModel.IsConnected = false;
        messageViewModel.SetMessage("Disconnected from spotify.");
    }

    /// <summary>
    /// This method is raised when the spotify service errors
    /// </summary>
    /// <param name="obj"></param>
    private void OnSpotifyServiceError(string obj)
    {
        messageViewModel.SetMessage(obj);
    }
}
