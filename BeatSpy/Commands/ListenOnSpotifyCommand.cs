using NLog;
using System;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class ListenOnSpotifyCommand : CommandBase
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public override void Execute(object? parameter)
    {
        if(parameter is BeatTrack currentTrack)
        {
            try
            {
                if(!string.IsNullOrEmpty(currentTrack.TrackUrl))
                {
                    BrowsUtil.OpenUrl(currentTrack.TrackUrl);
                    logger.Info($"Opening {currentTrack.TrackTitle} URL: {currentTrack.TrackUrl}");
                }
                else
                {
                    throw new ArgumentNullException(nameof(currentTrack.TrackUrl));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
