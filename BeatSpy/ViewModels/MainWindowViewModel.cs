using System;
using BeatSpy.Auth;
using BeatSpy.Commands;
using System.Windows.Input;
using BeatSpy.DataTypes.Constants;
using BeatSpy.DataTypes.Interfaces;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ObservableObject
{
    public ICommand? ConnectToSpotify { get; }
    public ICommand? ApplicationRedirect { get; }
    public ICommand? ListenOnSpotify { get; }
    public ICommand? SearchQueryEntered { get; }
    public ICommand? RandomTrack { get; }

    public BeatTrackViewModel Track => trackViewModel;
    public MessageHandlerViewModel MessageHandler => messageViewModel;

    private readonly BeatTrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;
    private readonly ISpotifyService spotify;

    public MainWindowViewModel()
    {
        messageViewModel = new();
        trackViewModel = new();
        spotify = new SpotifyPKCEAuth(messageViewModel, OnSpotifyConnected);
        ListenOnSpotify = new ListenOnSpotifyCommand();
        ConnectToSpotify = new ConnectToSpotifyCommand(spotify);
        SearchQueryEntered = new SearchTrackCommand(messageViewModel, trackViewModel, spotify);
        RandomTrack = new RandomTrackCommand(messageViewModel, trackViewModel, spotify);
        ApplicationRedirect = new RedirectToUrlCommand(DefaultConstants.LINK_GITHUB_PROFILE);
    }

    private void OnSpotifyConnected()
    {
        messageViewModel.DismissError.Execute(this);
        RandomTrack?.Execute(this);
    }
}
