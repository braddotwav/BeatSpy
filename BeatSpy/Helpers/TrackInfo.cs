using System;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Helpers;

internal static class TrackInfo
{
    public static string GetDuration(float durationMs) => TimeSpan.FromMilliseconds(durationMs).ToString("mm\\:ss");

    public static string GetVolume(float loudness) => $"{MathF.Round(loudness, 2)}Db";

    public static string GetTempo(float tempo) => $"{MathF.Round(tempo)}BPM";

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