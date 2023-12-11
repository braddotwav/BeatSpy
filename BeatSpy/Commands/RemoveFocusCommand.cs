using BeatSpy.Commands.Base;
using System.Windows;
using System.Windows.Input;

namespace BeatSpy.Commands;

internal class RemoveFocusCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is DependencyObject depObj)
        {
            //Clear the focus of focused object
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(depObj), null);

            //Clear keyboard focus
            Keyboard.ClearFocus();
        }
    }
}