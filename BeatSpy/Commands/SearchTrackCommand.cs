using NLog;
using System;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Services;
using BeatSpy.ViewModels;
using System.Windows.Input;
using BeatSpy.Commands.Base;
using System.Threading.Tasks;
using System.Windows.Controls;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Commands;

internal class SearchTrackCommand : AsyncCommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly ISpotifyService spotify;
    private readonly IMessageNotify messageNotify;
    private readonly TrackViewModel trackViewModel;

    public SearchTrackCommand(TrackViewModel trackViewModel, IMessageNotify messageNotify, ISpotifyService spotify)
    {
        this.spotify = spotify;
        this.messageNotify = messageNotify;
        this.trackViewModel = trackViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        if(parameter is TextBox searchQuery)
        {
            return !string.IsNullOrEmpty(searchQuery.Text);
        }
        else
        {
            return false;
        }
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        if (parameter is TextBox searchQuery)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(searchQuery), null);
            try
            {
                logger.Info($"Searching for {searchQuery.Text}");
                var fetchedTrack = await SearchTrack(spotify.Client, searchQuery.Text);
                trackViewModel.Track = fetchedTrack;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                logger.Error(ex, $"Could not find a track matching {searchQuery.Text}");
                messageNotify.SetMessage($"Could not find a track matching {searchQuery.Text}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Search request failed, please login with spotify.");
                messageNotify.SetMessage("Search request failed, please login with spotify.");
            }
        }
    }

    private static async Task<BeatTrack> SearchTrack(SpotifyClient client, string search)
    {
        if (client is not null)
        {
            var response = await client.Search.Item(new SearchRequest(SearchRequest.Types.Track, search));
            var track = await client.Tracks.Get(response.Tracks.Items[0].Id);
            var trackFeatures = await client.Tracks.GetAudioFeatures(track.Id);
            logger.Info($"Successfully found a result for {search}");

            return new BeatTrack(track, trackFeatures);
        }
        else
        {
            throw new ArgumentNullException(nameof(client));
        }
    }
}
