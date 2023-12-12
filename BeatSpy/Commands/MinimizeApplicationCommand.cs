using BeatSpy.Commands.Base;
using System.Windows;

namespace BeatSpy.Commands;

internal class MinimizeApplicationCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }
}