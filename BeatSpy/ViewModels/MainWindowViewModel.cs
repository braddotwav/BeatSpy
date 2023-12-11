using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;
using BeatSpy.DataTypes.Enums;
using BeatSpy.DataTypes.Constants;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ViewModelBase, IApplicationCommands
{
    public bool IsLoggedIn => spotify.IsLoggedIn;

    public ICommand RemoveFocus { get; }
    public ICommand ExitApplication { get; }
    public ICommand MinimizeApplication { get; }

    public ICommand ListenOnSpotify { get; }
    public ICommand SearchQueryEntered { get; }
    private ICommand RandomTrack { get; }

    public TrackViewModel TrackViewModel => trackViewModel;
    public MessageHandlerViewModel MessageHandler => messageViewModel;
    public ContextMenuViewModel ContextMenuViewModel => contextMenuViewModel;

    private readonly TrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;
    private readonly ContextMenuViewModel contextMenuViewModel;

    //Services
    private readonly IMessageDisplayService messageService;
    private readonly ISpotifyService spotify;

    public MainWindowViewModel(ISpotifyService spotifyService, IMessageDisplayService messageDisplayService)
    {
        spotify = spotifyService;
        messageService = messageDisplayService;
        spotify.OnServiceStateChanged += OnSpotifyServiceStateChanged;
        messageViewModel = new(messageService);
        trackViewModel = new();
        contextMenuViewModel = new(spotify, messageService);
        ExitApplication = new ExitApplicationCommand();
        MinimizeApplication = new MinimizeApplicationCommand();
        ListenOnSpotify = new ListenOnSpotifyCommand(spotify);
        RemoveFocus = new RemoveFocusCommand();
        SearchQueryEntered = new SearchTrackCommand(spotify, this, trackViewModel, messageService);
        RandomTrack = new RandomTrackCommand(spotify, trackViewModel, messageService);
    }

    /// <summary>
    /// Responds to changes in Spotify service state
    /// </summary>
    /// <param name="state">Client state</param>
    private void OnSpotifyServiceStateChanged(ConnectionType state)
    {
        switch (state)
        {
            case ConnectionType.Connected:
                RandomTrack.Execute(this);
                messageService.ClearMessage();
                messageService.DisplayInfoMessage(MessageType.Silent, LogConstants.CLIENT_CONNECTED);
                break;
            case ConnectionType.Disconnected:
                messageService.DisplayInfoMessage(MessageType.Normal, LogConstants.CLIENT_DISCONNECTED);
                break;
        }

        OnPropertyChanged(nameof(IsLoggedIn));
    }

    /// <summary>
    /// Dispose of any application resources
    /// </summary>
    public override void Dispose()
    {
        spotify.OnServiceStateChanged -= OnSpotifyServiceStateChanged;
        messageViewModel.Dispose();
        base.Dispose();
    }
}
