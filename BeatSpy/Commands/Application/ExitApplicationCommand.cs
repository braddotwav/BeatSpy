using System.Windows;
using BeatSpy.Commands.Base;

namespace BeatSpy.Commands;

internal sealed class ExitApplicationCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }
}
