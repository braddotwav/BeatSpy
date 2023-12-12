using BeatSpy.DataTypes.Enums;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.ViewModels.Base;
using System;
using System.Windows;

namespace BeatSpy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ViewModelBase? mainViewModel;

    protected override async void OnStartup(StartupEventArgs e)
    {
        //Set up spotify service
        IMessageDisplayService messageDisplayService = new MessageDisplayService();
        ISpotifyAuthenticationService spotifyAuth = new SpotifyAuthenticationService();
        ISpotifyService spotifyService = new SpotifyService(spotifyAuth);

        //Create main window and data context
        MainWindow mainWindow = new();
        mainViewModel = new MainWindowViewModel(spotifyService, messageDisplayService);
        mainWindow.DataContext = mainViewModel;

        //Try and connect to spotify
        try
        {
            await spotifyService.LogIn(LoginType.Automatic);
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
        base.OnExit(e);
    }
}
