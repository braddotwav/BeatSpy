using System.Windows;
using BeatSpy.Services;
using BeatSpy.ViewModels;

namespace BeatSpy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ISpotifyService spotifyService = new SpotifyService();

        MainWindow mainWindow = new();
        MainWindowViewModel mainViewModel = new(spotifyService);
        mainWindow.DataContext = mainViewModel;

        await spotifyService.Connect();

        mainWindow.Show();
    }
}
