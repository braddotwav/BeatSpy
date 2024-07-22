using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal sealed class OpenBrowserCommand : CommandBase
{
    private readonly IMessageDisplayService messageDisplayService;

    public OpenBrowserCommand(IMessageDisplayService messageService)
    {
        this.messageDisplayService = messageService;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            if (parameter is string url)
            {
                BrowserHelper.OpenURLInBrowser(url);
            }
        }
        catch (System.Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}