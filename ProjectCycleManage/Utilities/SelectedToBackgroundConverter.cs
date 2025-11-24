using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectCycleManage.Utilities
{
    public class SelectedToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8edff"));
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}