using System;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Helpers;

internal static class TrackInfo
{
    public static string GetDuration(float DurationMs)
    {
        var time = TimeSpan.FromMilliseconds(DurationMs);
        return $"{time.Minutes}:{MathF.Round(time.Seconds)}";
    }

    public static string GetVolume(float Loudness)
    {
        var roundedLoudness = MathF.Round(Loudness, 2);
        return $"{roundedLoudness}Db";
    }

    public static string GetTempo(float Tempo)
    {
        var roundedTempo = MathF.Round(Tempo);
        return $"{roundedTempo}BPM";
    }

    public static string GetKey(int KeyIndex)
    {
        return (Pitches)KeyIndex! switch
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
            _ => "Empty",
        };
    }
}