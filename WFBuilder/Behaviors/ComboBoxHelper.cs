using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WFBuilder.Behaviors
{
    public class ComboBoxHelper : Behavior<ComboBoxEdit>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var context = this.AssociatedObject.DataContext as RowData;
            if (context.Definition?.Path == "PinID")
            {
                var rowValue = PropertyGridHelper.GetPropertyGrid(this.AssociatedObject).GetRowValueByRowPath(context.FullPath.Replace("PinID", "AdapterID"));
                if (rowValue != null)
                    this.AssociatedObject.ItemsSource = MainWindow.Instance.GetInputPins((int)rowValue);
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.OnDetaching();
        }
    }
}
