using System;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Helpers;

internal static class TrackInfo
{
    /// <summary>
    /// Converts a duration in milliseconds into a formatted string represting minutes and seconds
    /// </summary>
    /// <param name="durationMs">Duration of the track in milliseconds</param>
    /// <returns></returns>
    public static string GetDuration(float durationMs) => TimeSpan.FromMilliseconds(durationMs).ToString("mm\\:ss");

    /// <summary>
    /// Converts a loudness value to a string representation in decibels (dB) rounded
    /// </summary>
    /// <param name="loudness">Loudness of the track</param>
    /// <returns></returns>
    public static string GetVolume(float loudness) => $"{MathF.Round(loudness, 2)}Db";

    /// <summary>
    /// Converts a tempo value to a string representation and rounds to the nearest whole number
    /// </summary>
    /// <param name="tempo">Tempo of the track</param>
    /// <returns></returns>
    public static string GetTempo(float tempo) => $"{MathF.Round(tempo)}BPM";

    /// <summary>
    /// Maps a pitch ID to its respective musical key or note.
    /// </summary>
    /// <param name="pitchId">Pitch ID of the track</param>
    /// <returns></returns>
    public static string GetKey(int pitchId) => (Pitches)pitchId switch
    {
        Pitches.PITCH_C => "C",
        Pitches.PITCH_CSHARP => "C#/D♭",
        Pitches.PITCH_D => "D",
        Pitches.PITCH_DSHARP => "D#/E♭",
        Pitches.PITCH_E => "E",
        Pitches.PITCH_F => "F",
        Pitches.PITCH_FSHARP => "F#/G♭",
        Pitches.PITCH_G => "G",
        Pitches.PITCH_GSHARP => "G#/A♭",
        Pitches.PITCH_A => "A",
        Pitches.PITCH_ASHARP => "A#/B♭",
        Pitches.PITCH_B => "B",
        _ => string.Empty,
    };
}