using System;
using SpotifyAPI.Web;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;

namespace BeatSpy.Commands.Spotify;

internal sealed class RandomTrackFromPlaylistCommand(string playlistId, MainWindowViewModel mainViewModel, ISpotifyService spotifyService, IMessageDisplayService messageDisplayService) : AsyncCommandBase
{
    private readonly string playlistId = playlistId;

    private readonly IMessageDisplayService messageDisplayService = messageDisplayService;
    private readonly ISpotifyService spotifyService = spotifyService;

    private readonly MainWindowViewModel mainViewModel = mainViewModel;

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            FullTrack track = await spotifyService.GetRandomTrackFromPlaylistAsync(playlistId);
            TrackAudioFeatures features = await spotifyService.GetAudioTrackFeaturesAsync(track.Id);

            mainViewModel.SetTrack(track, features);
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}