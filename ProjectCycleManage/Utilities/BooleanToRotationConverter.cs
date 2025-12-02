using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectCycleManage.Utilities
{
    public class BooleanToRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible && isVisible)
            {
                return 90.0; // 展开时旋转90度
            }
            return 0.0; // 收起时不旋转
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}