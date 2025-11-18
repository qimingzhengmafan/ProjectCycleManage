using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace ProjectCycleManage.Utilities
{
    public class BoolToBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                return new Thickness(3, 0, 0, 0); // 选中时的左边框厚度
            }
            return new Thickness(0); // 未选中时的边框厚度
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}