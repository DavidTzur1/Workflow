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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private DiagramControl _Diagram;

        public PropertyGridControlEx()
        {
            Loaded += PropertyGridControlEx_Loaded;
            CellValueChanged += PropertyGridControlEx_CellValueChanged;
            
            
            
        }

      

        private void PropertyGridControlEx_CellValueChanged(object sender, CellValueChangedEventArgs args)
        {
            
           log.Debug($" args.Row.FullPath={args.Row.FullPath} CellValueChanged OldValue={args.OldValue} NewValue={args.NewValue}");
            if(args.Row.Path == "AdapterID")
            {
                MainWindow.Instance._inputPins = MainWindow.Instance.GetInputPins((int)args.NewValue);
                (sender as PropertyGridControl).SetRowValueByRowPath("EntryPoints.[0].PinID", 1);
                this.GetRowValueByRowPath("EntryPoints.[0].PinID");
                

            }
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
