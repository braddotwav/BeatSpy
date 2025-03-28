using System;
using System.Windows;
using System.Threading;
using BeatSpy.ViewModels;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BeatSpy.Services;

namespace BeatSpy;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private MainWindowViewModel? mainWindowViewModel;

    private readonly Mutex mutex;

    public App()
    {
        mutex = new Mutex(true, "BeatSpy", out bool isNewInstance);

        if (!isNewInstance)
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    ShowWindow(process.MainWindowHandle, 9);
                    SetForegroundWindow(process.MainWindowHandle);
                    Current.Shutdown();
                }
            }
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        SpotifyService spotifyService = new();
        NotificationService notificationService = new();

        mainWindowViewModel = new(spotifyService, notificationService);
        MainWindow mainWindow = new()
        {
            DataContext = mainWindowViewModel,
        };

        await spotifyService.ConnectAsync();

        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        mutex.ReleaseMutex();
        mutex.Dispose();
        mainWindowViewModel?.Dispose();
    }
}
