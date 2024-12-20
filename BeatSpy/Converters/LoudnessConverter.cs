using System;
using System.Windows.Data;
using System.Globalization;

namespace BeatSpy.Converters;

internal sealed class LoudnessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is float loudness)
        {
            return $"{MathF.Round(loudness, 2)}Db";
        }

        return "0Db";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}