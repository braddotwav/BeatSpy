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

internal sealed class SearchTrackCommand : AsyncCommandBase
{
    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;
    private readonly MainWindowViewModel mainViewModel;

    private string currentSearched = string.Empty;
    private string searchQuery = string.Empty;

    public SearchTrackCommand(MainWindowViewModel mainViewModel, ISpotifyService spotifyService, IMessageDisplayService messageDisplayService)
    {
        this.mainViewModel = mainViewModel;
        this.messageDisplayService = messageDisplayService;
        this.spotifyService = spotifyService;
    }

    public override bool CanExecute(object? parameter)
    {
        searchQuery = ((TextBox)parameter!).Text;

        return spotifyService.IsLoggedIn && IsQueryValidAndNew(searchQuery);
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            currentSearched = searchQuery;
            FullTrack track = await spotifyService.GetTrackAsync(searchQuery);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeaturesAsync(track.Id);

            mainViewModel.SetTrack(new BeatTrack(track, features));
            ApplicationHelper.RemoveElementFocus(parameter);
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }

    private bool IsQueryValidAndNew(string query)
    {
        return !string.IsNullOrEmpty(query) && !string.Equals(query, currentSearched);
    }
}