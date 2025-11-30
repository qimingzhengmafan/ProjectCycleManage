using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectCycleManage.Utilities
{
    /// <summary>
    /// 数值格式化转换器
    /// 处理TextBox中数值的显示和输入转换
    /// </summary>
    public class NumericFormatConverter : IValueConverter
    {
        /// <summary>
        /// 将数值转换为格式化字符串（显示时使用）
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            
            if (value is double doubleValue)
            {
                // 显示时格式化为货币格式（¥ 1,000,000）
                return $"¥ {doubleValue:N0}";
            }
            
            if (value is int intValue)
            {
                // 显示时格式化为货币格式（¥ 1,000,000）
                return $"¥ {intValue:N0}";
            }
            
            return value.ToString();
        }

        /// <summary>
        /// 将格式化字符串转换为数值（输入时使用）
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return 0.0;

            string stringValue = value.ToString();
            
            // 移除货币符号和千位分隔符
            stringValue = stringValue.Replace("¥", "").Replace(",", "").Trim();
            
            // 尝试解析为double
            if (double.TryParse(stringValue, NumberStyles.Any, culture, out double result))
            {
                return result;
            }
            
            // 如果解析失败，返回0
            return 0.0;
        }
    }
}