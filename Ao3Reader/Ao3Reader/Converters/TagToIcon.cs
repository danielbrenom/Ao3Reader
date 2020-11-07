using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Ao3Reader.Converters
{
    public class TagToIcon: IValueConverter
    {
        private Dictionary<string, string> PatternToIcon = new Dictionary<string, string>
        {
            {"Choose Not To Use Archive Warnings", "\uf071"},
            {"No Archive Warnings Apply", "\uf12a"},
            {"Explicit", "\uf05e"},
            {"Graphic Depictions Of Violence", "\uf255"},
            {"Major Character Death", "\uf54c"},
            {"Rape/Non-Con", "\uf4b3"},
            {"Underage", "\uf06a"},
            {"General audiences", "\uf0c0"},
            {"Teen And Up Audiences", "\uf1ae"},
            {"Mature", "\uf508"},
            {"Gen", "\uf22d"},
            {"Multi", "\uf534"},
            {"F/M", "\uf228"},
            {"M/M", "\uf227"},
            {"F/F", "\uf226"},
            {"No category", "\uf00d"},
            {"Not Rated", "\uf128"},
            {"Other", "\uf0c8"},
            {"Complete Work", "\uf00c"},
            {"Work in Progress", "\uf252"}
        };
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            foreach (var pattern in PatternToIcon.Where(pattern => Regex.IsMatch(text ?? "", pattern.Key)))
            {
                text = pattern.Value;
                break;
            }
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            foreach (var pattern in PatternToIcon.Where(pattern => Regex.IsMatch(text ?? "", pattern.Value)))
            {
                text = pattern.Key;
                break;
            }
            return text;
        }
    }
}