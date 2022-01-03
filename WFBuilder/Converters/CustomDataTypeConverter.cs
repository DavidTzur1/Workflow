using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using WFBuilder.Attributes;

namespace WFBuilder.Converters
{
    public class CustomDataTypeConverter : MarkupExtension, IMultiValueConverter
    {

        public string InvocationMethod { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string propertyName && values[1] is string path && values[2] is PropertyGridControl propertyGrid)
            {

                var property = propertyGrid.GetRowValueByRowPath(propertyGrid.GetParentPath(path));
                var propertyInfo = TypeDescriptor.GetProperties(property).OfType<PropertyDescriptor>().First(x => x.Name == propertyName);
                var param = propertyInfo.Attributes.OfType<CustomDataTypeAttribute>().FirstOrDefault()?.Parameter;

                var context = DiagramControl.GetDiagram(propertyGrid)?.DataContext;
                if (context != null)
                    return context.GetType().GetMethod(InvocationMethod)?.Invoke(context, new object[] { param });
            }
            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
