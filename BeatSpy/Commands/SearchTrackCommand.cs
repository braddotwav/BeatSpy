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
using BeatSpy.DataTypes.Constants;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Commands;

internal class SearchTrackCommand : AsyncCommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly MainWindowViewModel mainViewModel;
    private readonly ISpotifyService spotify;
    private readonly IMessageNotify messageNotify;
    private readonly TrackViewModel trackViewModel;

    public SearchTrackCommand(MainWindowViewModel mainViewModel, TrackViewModel trackViewModel, IMessageNotify messageNotify, ISpotifyService spotify)
    {
        this.mainViewModel = mainViewModel;
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
            try
            {
                var fetchedTrack = await SearchTrack(spotify.Client, searchQuery.Text);
                mainViewModel.RemoveFocus!.Execute(searchQuery);
                trackViewModel.Track = fetchedTrack;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var message = string.Join(" ", LogInfoConstants.LOG_SEARCH_OUTOFRANGE, searchQuery.Text);
                logger.Error(ex, message);
                messageNotify.SetMessage(message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, LogInfoConstants.LOG_SEARCH_FAILED);
                messageNotify.SetMessage(LogInfoConstants.LOG_SEARCH_FAILED);
            }
        }
    }

    /// <summary>
    /// Returns a new BeatTrack object
    /// </summary>
    /// <param name="client">Spotify Client</param>
    /// <param name="search">The query to search for</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static async Task<BeatTrack> SearchTrack(SpotifyClient client, string search)
    {
        if (client is not null)
        {
            var response = await client.Search.Item(new SearchRequest(SearchRequest.Types.Track, search));
            var track = await client.Tracks.Get(response.Tracks.Items[0].Id);
            var trackFeatures = await client.Tracks.GetAudioFeatures(track.Id);
            logger.Info(string.Join(" ", LogInfoConstants.LOG_SEARCH_SUCCESS, search));

            return new BeatTrack(track, trackFeatures);
        }
        else
        {
            throw new ArgumentNullException(nameof(client));
        }
    }
}