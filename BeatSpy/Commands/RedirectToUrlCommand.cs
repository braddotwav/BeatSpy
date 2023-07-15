using BeatSpy.Helpers;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class RedirectToUrlCommand : CommandBase
{
    readonly string url = string.Empty;

    public RedirectToUrlCommand(string url)
    {
        this.url = url;
    }

    public override void Execute(object? parameter)
    {
        BrowsUtil.OpenUrl(url);
    }
}
