using BeatSpy.Services;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class DismissErrorCommand : CommandBase
{
    private readonly IMessageDisplayService messageDisplayService;

    public DismissErrorCommand(IMessageDisplayService messageDisplayService)
    {
        this.messageDisplayService = messageDisplayService;
    }

    public override bool CanExecute(object? parameter)
    {
        return !messageDisplayService.IsMessageEmpty;
    }

    public override void Execute(object? parameter)
    {
        messageDisplayService.ClearMessage();
    }
}
