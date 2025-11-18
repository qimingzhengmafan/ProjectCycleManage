using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectCycleManage.Utilities
{
    public class BoolToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                return new SolidColorBrush(Color.FromRgb(45, 91, 213)); // 选中时的边框颜色
            }
            return Brushes.Transparent; // 未选中时的边框颜色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}