﻿using System;
using System.Windows;
using System.Threading;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using System.Diagnostics;
using BeatSpy.ViewModels.Base;
using BeatSpy.DataTypes.Enums;
using System.Runtime.InteropServices;

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

    private ViewModelBase? mainViewModel;
    private readonly ISpotifyAuthenticationService spotifyAuth;

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
                    break;
                }
            }
        }

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
        mutex.ReleaseMutex();
        mutex.Dispose();
        mainViewModel!.Dispose();
        spotifyAuth.Dispose();
        base.OnExit(e);
    }
}
