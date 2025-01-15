using System;

namespace BeatSpy.Helpers;

internal static class RandomHelper
{
    private static readonly Random _random = new();

    /// <summary>
    /// Returns a random number within a set range
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int Range(int min, int max)
    {
        if (min >= max)
            throw new ArgumentException("The minimum number should be less than the maximum value");

        return _random.Next(min, max);
    }
}
