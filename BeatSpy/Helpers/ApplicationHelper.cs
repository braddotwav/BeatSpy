using System.Windows;
using System.Windows.Input;

namespace BeatSpy.Helpers;

public static class ApplicationHelper
{
    public static void RemoveElementFocus(object? parameter)
    {
        if (parameter is DependencyObject depObj)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(depObj), null);
            Keyboard.ClearFocus();
        }
    }
}