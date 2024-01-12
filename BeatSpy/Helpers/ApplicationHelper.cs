using System.Windows;
using System.Windows.Input;

namespace BeatSpy.Helpers;

public static class ApplicationHelper
{
    public static void RemoveElementFocus(object? parameter)
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