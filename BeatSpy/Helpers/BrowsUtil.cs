using System.Diagnostics;


namespace BeatSpy.Helpers;

internal static class BrowsUtil
{
    public static void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}
