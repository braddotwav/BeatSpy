using NLog;
using System;
using SpotifyAPI.Web;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;

namespace BeatSpy.Commands;

internal class RandomTrackCommand : AsyncCommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly ISpotifyService spotify;
    private readonly TrackViewModel trackViewModel;

    public RandomTrackCommand(TrackViewModel trackViewModel, ISpotifyService spotify)
    {
        this.spotify = spotify;
        this.trackViewModel = trackViewModel;
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        try
        {
            var fetchedTrack = await GetRandomTrack(spotify.Client);
            trackViewModel.Track = fetchedTrack;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Random track request failed");
        }
    }

    private static async Task<BeatTrack> GetRandomTrack(SpotifyClient client)
    {
        if (client is not null)
        {
            var top = await client.Playlists.Get("37i9dQZEVXbMDoHDwVN2tF");
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
