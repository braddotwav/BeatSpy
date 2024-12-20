using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BeatSpy.Converters;

internal sealed class InvertableBooleanToVisibilityConverter : IValueConverter
{
    enum VisibilityParameter
    {
        Normal,
        Inverted
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var boolValue = (bool)value;
        var direction = (VisibilityParameter)Enum.Parse(typeof(VisibilityParameter), (string)parameter);

        if (direction == VisibilityParameter.Inverted)
            return !boolValue ? Visibility.Visible : Visibility.Collapsed;

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}