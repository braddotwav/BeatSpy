using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeatSpy.Commands.Base;

internal abstract class AsyncCommandBase : ICommand
{
    protected bool isExecuting;
    public bool IsExecuting
    {
        get { return isExecuting; }
        set
        {
            isExecuting = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter)
    {
        return !isExecuting;
    }

    public async void Execute(object? parameter)
    {
        isExecuting = true;
        await ExcuteAsync(parameter);
        isExecuting = false;
    }

    protected abstract Task ExcuteAsync(object? parameter);
}
