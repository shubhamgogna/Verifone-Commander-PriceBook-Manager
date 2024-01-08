// -----------------------------------------------------------------------
// <copyright file="StringFormatConverter.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Converters
{
    using System;
    using System.Globalization;
    using Microsoft.UI.Xaml.Data;

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(
                    language == null ? CultureInfo.InvariantCulture : new CultureInfo(language),
                    formatString,
                    value);
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
