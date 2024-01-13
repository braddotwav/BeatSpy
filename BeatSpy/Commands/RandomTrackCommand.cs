using BeatSpy.Commands.Base;
using BeatSpy.Models;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace BeatSpy.Commands;

internal class RandomTrackCommand : AsyncCommandBase
{
    private readonly string playlistId = "37i9dQZEVXbMDoHDwVN2tF";

    private readonly IMessageDisplayService messageDisplayService;
    private readonly ISpotifyService spotifyService;
    private readonly TrackViewModel trackViewModel;

    public RandomTrackCommand(ISpotifyService spotifyService, TrackViewModel trackViewModel, IMessageDisplayService messageDisplayService)
    {
        this.spotifyService = spotifyService;
        this.trackViewModel = trackViewModel;
        this.messageDisplayService = messageDisplayService;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            FullTrack track = await spotifyService.GetRandomTrackFromPlaylistAsync(playlistId);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeaturesAsync(track.Id);

            trackViewModel.SetCurrentTrack(new BeatTrack(track, features));
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}