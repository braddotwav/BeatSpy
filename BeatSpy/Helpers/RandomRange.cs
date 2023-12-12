using System;

namespace BeatSpy.Helpers;

internal static class RandomRange
{
    private readonly static Random random = new();

    /// <summary>
    /// Returns a random range between a miniumum and maximum value
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns></returns>
    public static int Range(int min, int max)
    {
        return random.Next(min, max);
    }
}
