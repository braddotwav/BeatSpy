using System;
using System.Windows.Data;
using System.Globalization;

namespace BeatSpy.Converters;

internal sealed class TempoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is float tempo)
        {
            return $"{MathF.Round(tempo)} BPM";
        }

        return "0 BPM";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}