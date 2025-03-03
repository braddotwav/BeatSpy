using System;

namespace BeatSpy.Commands;

internal class DelegateCommand(Action<object> execute, Func<object, bool> canExecute) : CommandBase
{
    private readonly Action<object> _execute = execute;
    private readonly Func<object, bool> _canExecute = canExecute;

    public override void Execute(object? parameter)
    {
        _execute?.Invoke(parameter ?? EventArgs.Empty);
    }

    public override bool CanExecute(object? parameter)
    {
        return _canExecute(parameter ?? EventArgs.Empty);
    }
}