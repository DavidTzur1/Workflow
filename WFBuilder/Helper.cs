using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WFBuilder
{
    public static class Helper
    {
        public static readonly DependencyProperty ParameterProperty =
           DependencyProperty.RegisterAttached("Parameter", typeof(string), typeof(Helper), new PropertyMetadata());
        public static string GetParameter(DependencyObject obj)
        {
            return (string)obj.GetValue(ParameterProperty);
        }
        public static void SetParameter(DependencyObject obj, string value)
        {
            obj.SetValue(ParameterProperty, value);
        }
    }
}
