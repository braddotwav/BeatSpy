using System;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;

namespace BeatSpy.Commands;

internal class RandomTrackCommand : AsyncCommandBase
{
    private readonly string playlistId = "37i9dQZEVXbMDoHDwVN2tF";

    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;

    private readonly MainWindowViewModel mainViewModel;

    public RandomTrackCommand(MainWindowViewModel mainViewModel, ISpotifyService spotifyService, IMessageDisplayService messageDisplayService)
    {
        this.mainViewModel = mainViewModel;
        this.spotifyService = spotifyService;
        this.messageDisplayService = messageDisplayService;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            FullTrack track = await spotifyService.GetRandomTrackFromPlaylistAsync(playlistId);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeaturesAsync(track.Id);

            mainViewModel.SetTrack(new BeatTrack(track, features));
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}