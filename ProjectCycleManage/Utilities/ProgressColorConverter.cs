using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectCycleManage.Utilities
{
    /// <summary>
    /// 根据进度值返回对应的颜色
    /// </summary>
    public class ProgressColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int progress)
            {
                if (progress >= 60)
                {
                    // 良好 - 绿色
                    return Color.FromRgb(76, 175, 80); // #4CAF50
                }
                else if (progress >= 30)
                {
                    // 警告 - 橙色
                    return Color.FromRgb(255, 152, 0); // #FF9800
                }
                else
                {
                    // 严重 - 红色
                    return Color.FromRgb(244, 67, 54); // #F44336
                }
            }
            return Color.FromRgb(224, 224, 224); // 默认灰色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
