using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TreeViewTest
{
    public class TreeNodeTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeNodeType nodeType)
            {
                return nodeType switch
                {
                    TreeNodeType.Folder => "",     // 文件夹图标
                    TreeNodeType.Document => "",   // 文档图标
                    TreeNodeType.Image => "",      // 图片图标
                    TreeNodeType.CodeFile => "",   // 代码文件图标
                    _ => ""                        // 默认图标
                };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
