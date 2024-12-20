using System;
using System.Windows.Data;
using System.Globalization;

namespace BeatSpy.Converters;

internal sealed class DurationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int duration)
        {
            return TimeSpan.FromMilliseconds(duration).ToString("mm\\:ss");
        }

        return "0:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}