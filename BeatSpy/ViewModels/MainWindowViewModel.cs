using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;
using BeatSpy.DataTypes.Enums;
using BeatSpy.Commands.Spotify;
using BeatSpy.DataTypes.Constants;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ViewModelBase, IApplicationCommands
{
    public bool IsLoggedIn => spotify.IsLoggedIn;

    public Track? Track { get; private set; }
    public bool IsTrackEmpty => Track == null;

    #region Application Commands
    public ICommand RemoveFocusCommand { get; }
    public ICommand ExitApplicationCommand { get; }
    public ICommand MinimizeApplicationCommand { get; }
    public ICommand OpenInBrowserCommand { get; }
    #endregion

    #region Spotify Commands
    public ICommand SearchTrackCommand { get; }
    private ICommand RandomTrackCommand { get; }
    #endregion

    public MessageHandlerViewModel MessageHandler => messageViewModel;
    public ContextMenuViewModel ContextMenuViewModel => contextMenuViewModel;

    //Viewmodels
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
        contextMenuViewModel = new(spotify, messageService);
        ExitApplicationCommand = new ExitApplicationCommand();
        MinimizeApplicationCommand = new MinimizeApplicationCommand();
        RemoveFocusCommand = new RemoveElementFocusCommand();
        OpenInBrowserCommand = new OpenBrowserCommand(messageService);
        SearchTrackCommand = new SearchTrackCommand(this, spotify, messageService);
        RandomTrackCommand = new RandomTrackCommand(this, spotify, messageService);
    }

    public void SetTrack(FullTrack track, TrackAudioFeatures features)
    {
        Track = new Track(track, features);
        OnPropertyChanged(nameof(Track));
        OnPropertyChanged(nameof(IsTrackEmpty));
    }

    private void OnSpotifyServiceStateChanged(ConnectionType state)
    {
        switch (state)
        {
            case ConnectionType.Connected:
                RandomTrackCommand.Execute(this);
                messageService.ClearMessage();
                messageService.DisplayInfoMessage(LogConstants.CLIENT_CONNECTED, silent: true);
                break;
            case ConnectionType.Disconnected:
                messageService.DisplayInfoMessage(LogConstants.CLIENT_DISCONNECTED);
                break;
        }

        OnPropertyChanged(nameof(IsLoggedIn));
    }

    public override void Dispose()
    {
        spotify.OnServiceStateChanged -= OnSpotifyServiceStateChanged;
        messageViewModel.Dispose();
    }
}
