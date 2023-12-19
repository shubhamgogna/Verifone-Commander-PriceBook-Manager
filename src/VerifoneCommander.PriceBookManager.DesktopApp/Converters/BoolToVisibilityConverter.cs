// -----------------------------------------------------------------------
// <copyright file="BoolToVisibilityConverter.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Converters
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            if (value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }

            throw new ArgumentException($"Unknown type of value '{value?.GetType()}'");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
