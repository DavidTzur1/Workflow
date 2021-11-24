using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WFBuilder
{
    public class PropertyGridControlEx : PropertyGridControl
    {

        private DiagramControl _Diagram;

        public PropertyGridControlEx()
        {
            Loaded += PropertyGridControlEx_Loaded;
        }

        private void PropertyGridControlEx_Loaded(object sender, RoutedEventArgs e)
        {
            _Diagram = DiagramControl.GetDiagram(this);
            _Diagram.PreviewMouseDown += Diagram_PreviewMouseDown;
        }

        private void Diagram_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var parentPanel = DockLayoutManager.GetLayoutItem((DependencyObject)e.OriginalSource);
            if (parentPanel == null || parentPanel?.Name != "PART_PropertiesPane")
                foreach (RowData row in this.RowDataGenerator.View.Items)
                    if (row.ValidationError != null)
                    {
                        e.Handled = true;
                        return;
                    }
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                foreach (RowData row in this.RowDataGenerator.View.Items)
                    if (row.ValidationError != null)
                        this.Focus();
            }
            else base.OnIsKeyboardFocusWithinChanged(e);
        }
    }
}
