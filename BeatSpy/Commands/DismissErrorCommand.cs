using BeatSpy.ViewModels;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal class DismissErrorCommand : CommandBase
{
    private readonly MessageHandlerViewModel messageViewModel;

    public DismissErrorCommand(MessageHandlerViewModel messageViewModel)
    {
        this.messageViewModel = messageViewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        return !messageViewModel.IsMessageEmpty;
    }

    public override void Execute(object? parameter)
    {
        messageViewModel.ClearMessage();
    }
}
