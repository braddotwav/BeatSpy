using NLog;
using System;
using BeatSpy.Models;
using BeatSpy.Helpers;
using BeatSpy.Commands.Base;
using BeatSpy.DataTypes.Constants;

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
                    logger.Info(string.Join(" ", LogInfoConstants.LOG_COMMAND_OPENSPOTIFY, currentTrack.TrackTitle));
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
