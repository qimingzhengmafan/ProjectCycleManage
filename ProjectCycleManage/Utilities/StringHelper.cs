using System;

namespace ProjectCycleManage.Utilities
{
    /// <summary>
    /// 字符串处理工具类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 获取字符串的第一个字符，如果为空则返回"N"
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>第一个字符或"N"</returns>
        public static string GetFirstCharOrN(string input)
        {
            // 空检查
            if (string.IsNullOrEmpty(input))
            {
                return "N";
            }
            
            // 截取第一个字符并返回
            return input.Substring(0, 1);
        }
    }
}