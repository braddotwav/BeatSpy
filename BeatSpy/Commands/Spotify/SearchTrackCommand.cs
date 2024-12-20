using System;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeatSpy.Commands.Spotify;

internal sealed class SearchTrackCommand(MainWindowViewModel mainViewModel, ISpotifyService spotifyService, IMessageDisplayService messageDisplayService) : AsyncCommandBase
{
    private readonly IMessageDisplayService messageDisplayService = messageDisplayService;
    private readonly ISpotifyService spotifyService = spotifyService;
    private readonly MainWindowViewModel mainViewModel = mainViewModel;

    private string currentSearched = string.Empty;
    private string searchQuery = string.Empty;

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

            mainViewModel.SetTrack(track, features);
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