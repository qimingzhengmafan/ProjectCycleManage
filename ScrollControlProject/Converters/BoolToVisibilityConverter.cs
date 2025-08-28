using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScrollControlProject.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        // 当从ViewModel到View时调用
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // 处理参数：可以反转逻辑
                if (parameter != null && parameter.ToString().Equals("Collapsed", StringComparison.OrdinalIgnoreCase))
                {
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                }
                if (parameter != null && parameter.ToString().Equals("Hidden", StringComparison.OrdinalIgnoreCase))
                {
                    return boolValue ? Visibility.Hidden : Visibility.Visible;
                }

                // 默认行为：true=Visible, false=Collapsed
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        // 当从View到ViewModel时调用（双向绑定时需要）
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
