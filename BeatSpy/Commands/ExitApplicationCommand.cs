using BeatSpy.Commands.Base;
using System.Windows;

namespace BeatSpy.Commands;

internal class ExitApplicationCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }
}
