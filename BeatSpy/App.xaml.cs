using System;
using System.Windows;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.ViewModels.Base;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ViewModelBase? mainViewModel;
    private readonly ISpotifyAuthenticationService spotifyAuth;

    public App()
    {
        spotifyAuth = new SpotifyAuthenticationService();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        //Set up spotify service
        ISpotifyService spotifyService = new SpotifyService(spotifyAuth);
        IMessageDisplayService messageDisplayService = new MessageDisplayService();

        //Create main window and data context
        MainWindow mainWindow = new();
        mainViewModel = new MainWindowViewModel(spotifyService, messageDisplayService);
        mainWindow.DataContext = mainViewModel;

        //Try and connect to spotify
        try
        {
            await spotifyService.LoginAsync(LoginType.Automatic);
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }

        //Make main window visable
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        mainViewModel!.Dispose();
        spotifyAuth.Dispose();
        base.OnExit(e);
    }
}
