using System;

namespace BeatSpy.Helpers;

internal static class RandomHelper
{
    private static readonly Random _random = new();

    /// <summary>
    /// Returns a random range between a miniumum and maximum value, both values are incluesive
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns></returns>
    public static int Range(int minInclusive, int maxInclusive)
    {
        if (minInclusive >= maxInclusive)
            throw new ArgumentException("The minimum number should be less than the maximum value");
         
        return _random.Next(minInclusive, maxInclusive + 1);
    }
}
