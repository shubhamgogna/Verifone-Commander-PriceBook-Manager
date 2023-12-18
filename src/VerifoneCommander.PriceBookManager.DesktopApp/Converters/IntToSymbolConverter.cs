using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace VerifoneCommander.PriceBookManager.DesktopApp.Converters
{
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
