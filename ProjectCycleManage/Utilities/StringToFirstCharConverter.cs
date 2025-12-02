using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectCycleManage.Utilities
{
    public class StringToFirstCharConverter : IValueConverter
    {
        public static readonly StringToFirstCharConverter Default = new StringToFirstCharConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                // 返回字符串的第一个字符
                return str.Substring(0, 1);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}