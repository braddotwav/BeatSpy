using NLog;
using System;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Commands;

internal class RandomTrackCommand : AsyncCommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly ISpotifyService service;
    private readonly BeatTrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;

    public RandomTrackCommand(MessageHandlerViewModel messageViewModel, BeatTrackViewModel trackViewModel, ISpotifyService service)
    {
        this.service = service;
        this.trackViewModel = trackViewModel;
        this.messageViewModel = messageViewModel;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            var track = await GetRandomTrack(service.Client);
            trackViewModel.CurrentTrack = track;
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            messageViewModel.Message = "Error: Issue retriving a random track";
        }
    }

    private static async Task<BeatTrack> GetRandomTrack(SpotifyClient client)
    {
        if (client is not null)
        {
            var top = await client.Playlists.Get("37i9dQZEVXbMDoHDwVN2tF"); //Top 50 playlist
            var track = top.Tracks.Items[RandomRange.Range(0, top.Tracks.Items.Count)].Track as FullTrack;
            var trackFeatures = await client.Tracks.GetAudioFeatures(track.Id);
            logger.Info($"Successfully retrieved {track.Name} as a random song");

            return new BeatTrack(track, trackFeatures);
        }
        else
        {
            throw new ArgumentNullException(nameof(client));
        }
    }
}
