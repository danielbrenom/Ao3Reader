using System;
using System.Globalization;
using Ao3Domain.Helpers;
using Xamarin.Forms;

namespace Ao3Reader.Converters
{
    public class TextSanitizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Sanitizer.TextSanitizer(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string;
        }
    }
}