﻿using System;
using System.Windows.Data;
using System.Globalization;

namespace BeatSpy.Converters;

internal sealed class KeyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int pitchId)
        {
            return pitchId switch
            {
                0 => "C",
                1 => "C♯/D♭",
                2 => "D",
                3 => "D♯/E♭",
                4 => "E",
                5 => "F",
                6 => "F♯/G♭",
                7 => "G",
                8 => "G♯/A♭",
                9 => "A",
                10 => "A♯/B♭",
                11 => "B",
                _ => string.Empty,
            };
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
