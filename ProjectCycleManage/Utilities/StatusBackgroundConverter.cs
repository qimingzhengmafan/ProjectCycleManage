using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectCycleManage.Utilities
{
    /// <summary>
    /// 根据健康状态返回背景颜色
    /// </summary>
    public class StatusBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "良好":
                        return new SolidColorBrush(Color.FromRgb(232, 245, 233)); // #E8F5E9
                    case "警告":
                        return new SolidColorBrush(Color.FromRgb(255, 248, 225)); // #FFF8E1
                    case "严重":
                        return new SolidColorBrush(Color.FromRgb(255, 235, 238)); // #FFEBEE
                    default:
                        return new SolidColorBrush(Color.FromRgb(245, 245, 245)); // 默认灰色
                }
            }
            return new SolidColorBrush(Color.FromRgb(245, 245, 245));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
