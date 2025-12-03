using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectCycleManage.Utilities
{
    /// <summary>
    /// 根据健康状态返回文字颜色
    /// </summary>
    public class StatusForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "良好":
                        return new SolidColorBrush(Color.FromRgb(76, 175, 80)); // #4CAF50
                    case "警告":
                        return new SolidColorBrush(Color.FromRgb(255, 152, 0)); // #FF9800
                    case "严重":
                        return new SolidColorBrush(Color.FromRgb(244, 67, 54)); // #F44336
                    default:
                        return new SolidColorBrush(Color.FromRgb(102, 102, 102)); // 默认灰色
                }
            }
            return new SolidColorBrush(Color.FromRgb(102, 102, 102));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
