using NLog;
using System;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class ListenOnSpotifyCommand : CommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private BeatTrack? currentTrack;

    public override void Execute(object? parameter)
    {
        currentTrack = parameter as BeatTrack;
        if (string.IsNullOrEmpty(currentTrack.TrackUrl)) { return; }
        try
        { 
            BrowsUtil.OpenUrl(currentTrack.TrackUrl);
            logger.Info($"Opening {currentTrack.TrackTitle} via spotify: {currentTrack.TrackUrl}");
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }
}
