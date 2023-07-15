using System;

namespace BeatSpy.Helpers;

internal static class RandomRange
{
    private readonly static Random random = new();

    /// <summary>
    /// Returns a random range between the min and max parameters
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Range(int min, int max)
    {
        return random.Next(min, max);
    }
}
