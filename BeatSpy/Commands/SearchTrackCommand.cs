using NLog;
using System;
using BeatSpy.Models;
using SpotifyAPI.Web;
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

    private readonly ISpotifyService service;
    private readonly BeatTrackViewModel trackViewModel;
    private readonly MessageHandlerViewModel messageViewModel;

    public SearchTrackCommand(MessageHandlerViewModel messageViewModel, BeatTrackViewModel trackViewModel, ISpotifyService service)
    {
        this.service = service;
        this.trackViewModel = trackViewModel;
        this.messageViewModel = messageViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        var searchQuery = parameter as TextBox;
        return !string.IsNullOrEmpty(searchQuery?.Text) && service.IsConnected();
    }

    protected override async Task ExcuteAsync(object? parameter)
    {
        if (parameter is TextBox searchQuery)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(searchQuery), null);
            try
            {
                logger.Info($"Searching for {searchQuery.Text}");
                var track = await SearchTrack(service.Client, searchQuery.Text);
                trackViewModel.CurrentTrack = track;
            }
            catch (ArgumentNullException ex)
            {
                logger.Error(ex, "Client is null");
                messageViewModel.SetMessage("Client is null - Please log-in using your spotify account");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                logger.Error(ex, $"Could not find a track matching {searchQuery.Text}");
                messageViewModel.SetMessage("Failed to find a track matching your request");
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
