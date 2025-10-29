using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectCycleManage.Model
{
    public class TextBoxBehavior
    {
        public static readonly DependencyProperty ClearTextOnClickProperty =
            DependencyProperty.RegisterAttached(
                "ClearTextOnClick",
                typeof(bool),
                typeof(TextBoxBehavior),
                new UIPropertyMetadata(false, OnClearTextOnClickChanged));

        public static bool GetClearTextOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearTextOnClickProperty);
        }

        public static void SetClearTextOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearTextOnClickProperty, value);
        }

        private static void OnClearTextOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewMouseLeftButtonDown += TextBox_PreviewMouseLeftButtonDown;
                }
                else
                {
                    textBox.PreviewMouseLeftButtonDown -= TextBox_PreviewMouseLeftButtonDown;
                }
            }
        }

        private static void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !textBox.IsKeyboardFocusWithin)
            {
                textBox.Clear();
                textBox.Focus();
                e.Handled = true;
            }
        }
    }
}
