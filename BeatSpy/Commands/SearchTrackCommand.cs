using System;
using SpotifyAPI.Web;
using BeatSpy.Models;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using System.Windows.Controls;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Commands;

internal class SearchTrackCommand : AsyncCommandBase
{
    private readonly IApplicationCommands applicationCommands;
    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;
    private readonly TrackViewModel trackViewModel;

    private string currentQuery = string.Empty;

    public SearchTrackCommand(ISpotifyService spotifyService, IApplicationCommands applicationCommands, TrackViewModel trackViewModel, IMessageDisplayService messageDisplayService)
    {
        this.messageDisplayService = messageDisplayService;
        this.applicationCommands = applicationCommands;
        this.spotifyService = spotifyService;
        this.trackViewModel = trackViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        string queryContent = ((TextBox)parameter!).Text;
        return spotifyService.IsLoggedIn && !string.IsNullOrEmpty(((TextBox)parameter!).Text) && !string.Equals(currentQuery, queryContent);
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            FullTrack track = await spotifyService.GetTrack(((TextBox)parameter!).Text);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeatures(track.Id);

            trackViewModel.SetCurrentTrack(new BeatTrack(track, features));
            currentQuery = ((TextBox)parameter!).Text;
            applicationCommands.RemoveFocus.Execute(parameter);
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}