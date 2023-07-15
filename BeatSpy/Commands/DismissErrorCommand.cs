using BeatSpy.Commands.Base;
using BeatSpy.ViewModels;

namespace BeatSpy.Commands;

internal class DismissErrorCommand : CommandBase
{
    private readonly MessageHandlerViewModel messageViewModel;

    public DismissErrorCommand(MessageHandlerViewModel messageViewModel)
    {
        this.messageViewModel = messageViewModel;
    }

    public override void Execute(object? parameter)
    {
        messageViewModel.Message = string.Empty;
    }

    public override bool CanExecute(object? parameter)
    {
        return !messageViewModel.IsMessageEmpty;
    }
}
