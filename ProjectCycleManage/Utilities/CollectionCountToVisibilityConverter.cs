using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectCycleManage.Utilities
{
    public class CollectionCountToVisibilityConverter : IValueConverter
    {
        private static CollectionCountToVisibilityConverter _instance;
        
        public static CollectionCountToVisibilityConverter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollectionCountToVisibilityConverter();
                }
                return _instance;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool showWhenEmpty = parameter as string == "inverse";
            
            if (value is ICollection collection)
            {
                if (showWhenEmpty)
                {
                    return collection.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            
            if (value is int count)
            {
                if (showWhenEmpty)
                {
                    return count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                return count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}