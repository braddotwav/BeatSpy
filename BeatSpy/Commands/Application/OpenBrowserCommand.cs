using System;
using BeatSpy.Helpers;
using BeatSpy.Services;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal sealed class OpenBrowserCommand(IMessageDisplayService messageService) : CommandBase
{
    private readonly IMessageDisplayService messageDisplayService = messageService;

    public override void Execute(object? parameter)
    {
        try
        {
            if (parameter is string url)
            {
                BrowserHelper.OpenURLInBrowser(url);
            }
        }
        catch (Exception ex)
        {
            messageDisplayService.DisplayErrorMessage(ex);
        }
    }
}