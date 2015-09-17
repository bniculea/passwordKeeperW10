using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PasswordKeeper.Converters
{
    public class ComboBoxSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return Visibility.Collapsed;
            string selectedIdem = value.ToString();
            return selectedIdem.Equals("Custom") ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
