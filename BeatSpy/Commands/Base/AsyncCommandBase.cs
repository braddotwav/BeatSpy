using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeatSpy.Commands.Base;

internal abstract class AsyncCommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        await ExcuteAsync(parameter);
    }

    protected abstract Task ExcuteAsync(object? parameter);
}
