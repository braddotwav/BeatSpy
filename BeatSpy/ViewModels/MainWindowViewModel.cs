using System.Windows;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Commands;
using System.Threading;
using BeatSpy.Services;
using System.Windows.Input;

namespace BeatSpy.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private bool canLogIn = true;

    public bool CanLogIn
    {
        get { return canLogIn; }
        set 
        { 
            canLogIn = value;
            OnPropertyChanged(nameof(CanLogIn));
        }
    }

    public bool IsLoggedIn => spotifyService.IsConnected;

    private Track? track;

    public Track? Track
    {
        get { return track; }
        set 
        {
            if (track != value)
            {
                track = value;
                OnPropertyChanged(nameof(Track));
                OnPropertyChanged(nameof(IsTrackEmpty));
            }
        }
    }

    public bool IsTrackEmpty => Track == null;

    public NotificationBannerViewModel NotificationBannerViewModel { get; }

    #region Commands
    public ICommand LogInCommand { get; }
    public ICommand LogOutCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand OpenInBrowserCommand { get; }
    #endregion

    private readonly ISpotifyService spotifyService;
    private readonly INotificationSerivce notificationSerivce;

    private string searchQuery = string.Empty;

    public MainWindowViewModel(ISpotifyService spotifyService, INotificationSerivce notificationSerivce)
    {
        this.spotifyService = spotifyService;
        spotifyService.OnConnectionStateChanged += SpotifyService_OnConnectionStateChanged;

        this.notificationSerivce = notificationSerivce;

        NotificationBannerViewModel = new NotificationBannerViewModel(notificationSerivce);

        LogInCommand = new DelegateCommand(ExecuteLogInCommand, (_) => CanLogIn);
        LogOutCommand = new DelegateCommand(ExecuteLogOutCommand, (_) => IsLoggedIn);
        SearchCommand = new DelegateCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
        OpenInBrowserCommand = new DelegateCommand(ExecuteOpenInBrowserCommand, (_) => true);
    }

    private async void SpotifyService_OnConnectionStateChanged(bool success)
    {
        OnPropertyChanged(nameof(IsLoggedIn));

        Track = success ? await spotifyService.GetRandomTrackFromPlaylistAsync("37i9dQZEVXbNG2KDcFcKOF") : null;
    }

    private async void ExecuteLogInCommand(object parameter)
    {
        CancellationTokenSource cts = new();

        notificationSerivce.ShowNotification(NotificationFactory.ProgressNotification(
            "Waiting for you to authenticate with Spotify…", () => cts.Cancel()));

        try
        {
            CanLogIn = false;
            bool authed = await spotifyService.AuthenticateAsync(cts.Token);

            if (authed)
                await spotifyService.ConnectAsync();

            notificationSerivce.ClearNotification();
        }
        catch
        {
            notificationSerivce.ShowNotification(NotificationFactory.ErrorNotification(
                $"Something went wrong while authenticating"));
        }

        CanLogIn = true;
    }

    private void ExecuteLogOutCommand(object parameter)
    {
        spotifyService.Disconnect();
        notificationSerivce.ShowNotification(NotificationFactory.InfoNotification(
            "Disconnected from Spotify"));
    }

    private async void ExecuteSearchCommand(object parameter)
    {
        try
        {
            Track = await spotifyService.GetTrackFromSearchAsync(searchQuery);
        }
        catch
        {
            notificationSerivce.ShowNotification(NotificationFactory.ErrorNotification(
                $"Something went wrong while searching for {searchQuery}"));
        }
    }

    private bool CanExecuteSearchCommand(object parameter)
    {
        if (parameter is not string query) return false;
        if (string.IsNullOrEmpty(query) || string.Equals(query, searchQuery)) return false;
        searchQuery = query;
        return true;
    }

    private void ExecuteOpenInBrowserCommand(object parameter)
    {
        if (parameter is not string url) return;

        try
        {
            BrowserHelper.OpenURLInBrowser(url);
        }
        catch
        {
            notificationSerivce.ShowNotification(NotificationFactory.ErrorNotification(
                "Something went wrong trying to open the browser"));
        }
    }

    public override void Dispose()
    {
        spotifyService.OnConnectionStateChanged -= SpotifyService_OnConnectionStateChanged;
        NotificationBannerViewModel?.Dispose();
    }
}
