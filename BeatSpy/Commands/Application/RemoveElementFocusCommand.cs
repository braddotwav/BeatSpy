using BeatSpy.Helpers;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal sealed class RemoveElementFocusCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        ApplicationHelper.RemoveElementFocus(parameter);
    }
}