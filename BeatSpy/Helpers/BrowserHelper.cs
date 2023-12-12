using System.Diagnostics;

namespace BeatSpy.Helpers;

public static class BrowserHelper
{
    /// <summary>
    /// Opens the default browser to visit a URL
    /// </summary>
    /// <param name="url">The URL to visit</param>
    public static void OpenURLInBrowser(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}