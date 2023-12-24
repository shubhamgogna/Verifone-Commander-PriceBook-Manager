// -----------------------------------------------------------------------
// <copyright file="IntToSymbolConverter.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Converters
{
    using System;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;

    public class IntToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            if (value is int)
            {
                return (Symbol)(int)value;
            }

            throw new ArgumentException($"Unknown type of value '{value?.GetType()}'");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
