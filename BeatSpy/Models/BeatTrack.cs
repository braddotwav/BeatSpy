using System;
using SpotifyAPI.Web;

namespace BeatSpy.Models;

internal sealed class BeatTrack
{
    public string? Artist { get; }
    public string? Title { get; set; }
    public string? CoverUrl { get; }
    public string? Url { get; }
    public string? Volume { get; }
    public string? Tempo { get; }
    public string? Key { get; }
    public string? Duration { get; }

    public BeatTrack(FullTrack SpotifyFullTrack, TrackAudioFeatures SpotifyAudioFeatures)
    {
        Artist = SpotifyFullTrack.Artists[0].Name;
        Title = SpotifyFullTrack.Name;
        CoverUrl = SpotifyFullTrack.Album.Images[0].Url;
        Url = SpotifyFullTrack.Uri;
        Volume = GetVolume(SpotifyAudioFeatures.Loudness);
        Tempo = GetTempo(SpotifyAudioFeatures.Tempo);
        Key = GetKey(SpotifyAudioFeatures.Key);
        Duration = GetDuration(SpotifyAudioFeatures.DurationMs);
    }

    /// <summary>
    /// Converts a duration in milliseconds into a formatted string represting minutes and seconds
    /// </summary>
    /// <param name="durationMs">Duration of the track in milliseconds</param>
    /// <returns></returns>
    private string GetDuration(float durationMs) => TimeSpan.FromMilliseconds(durationMs).ToString("mm\\:ss");

    /// <summary>
    /// Converts a loudness value to a string representation in decibels (dB) rounded
    /// </summary>
    /// <param name="loudness">Loudness of the track</param>
    /// <returns></returns>
    private string GetVolume(float loudness) => $"{MathF.Round(loudness, 2)}Db";

    /// <summary>
    /// Converts a tempo value to a string representation and rounds to the nearest whole number
    /// </summary>
    /// <param name="tempo">Tempo of the track</param>
    /// <returns></returns>
    private string GetTempo(float tempo) => $"{MathF.Round(tempo)}BPM";

    /// <summary>
    /// Maps a pitch ID to its respective musical key or note.
    /// </summary>
    /// <param name="pitchId">Pitch ID of the track</param>
    /// <returns></returns>
    private string GetKey(int pitchId) => (Pitches)pitchId switch
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

public enum Pitches
{
    PITCH_C = 0,
    PITCH_CSHARP = 1,
    PITCH_D = 2,
    PITCH_DSHARP = 3,
    PITCH_E = 4,
    PITCH_F = 5,
    PITCH_FSHARP = 6,
    PITCH_G = 7,
    PITCH_GSHARP = 8,
    PITCH_A = 9,
    PITCH_ASHARP = 10,
    PITCH_B = 11,
}