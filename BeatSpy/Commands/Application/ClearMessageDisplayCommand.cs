using BeatSpy.Services;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal sealed class ClearMessageDisplayCommand(IMessageDisplayService messageDisplayService) : CommandBase
{
    private readonly IMessageDisplayService messageDisplayService = messageDisplayService;

    public override bool CanExecute(object? parameter)
    {
        return !messageDisplayService.IsMessageEmpty;
    }

    public override void Execute(object? parameter)
    {
        messageDisplayService.ClearMessage();
    }
}
