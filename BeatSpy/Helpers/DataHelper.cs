﻿using System;
using System.IO;

namespace BeatSpy.Helpers;

internal static class DataHelper
{
    /// <summary>
    /// Generates and returns the complete file path for a specified file within the "BeatSpy" application folder
    /// </summary>
    /// <param name="fileName">The file name</param>
    /// <returns></returns>
    public static string GetFullFilePath(string fileName)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"BeatSpy\", fileName);
    }
}