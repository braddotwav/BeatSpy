using System;
using SpotifyAPI.Web;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeatSpy.Commands;

internal class SearchTrackCommand : AsyncCommandBase
{
    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;
    private readonly TrackViewModel trackViewModel;

    private string currentSearched = string.Empty;
    private string searchQuery = string.Empty;

    public SearchTrackCommand(ISpotifyService spotifyService, TrackViewModel trackViewModel, IMessageDisplayService messageDisplayService)
    {
        this.messageDisplayService = messageDisplayService;
        this.spotifyService = spotifyService;
        this.trackViewModel = trackViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        searchQuery = ((TextBox)parameter!).Text;

        return spotifyService.IsLoggedIn && !string.IsNullOrEmpty(searchQuery) && !string.Equals(searchQuery, currentSearched);
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            currentSearched = searchQuery;
            FullTrack track = await spotifyService.GetTrackAsync(searchQuery);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeaturesAsync(track.Id);

            trackViewModel.SetCurrentTrack(new BeatTrack(track, features));
            ApplicationHelper.RemoveElementFocus(parameter);
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}