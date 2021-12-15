using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            ShownEditor += PropertyGridControlEx_ShownEditor;



        }

        private void PropertyGridControlEx_ShownEditor(object sender, PropertyGridEditorEventArgs args)
        {
            if (args.Editor is ComboBoxEdit && args.Row.Path == "PinID")
            {
                var rowValue = ((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("PinID", "AdapterID"));
                if (rowValue != null)
                    ((ComboBoxEdit)args.Editor).ItemsSource = MainWindow.Instance.GetInputPins((int)rowValue);
            }
        }

        private void PropertyGridControlEx_CellValueChanged(object sender, CellValueChangedEventArgs args)
        {
            
           log.Debug($" args.Row.FullPath={args.Row.FullPath} CellValueChanged OldValue={args.OldValue} NewValue={args.NewValue}");
            if (args.Row.Path == "AdapterID")
            {            
                /////////////Update the BackgroundInputPoint

                int pinID = (int)((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("AdapterID", "PinID"));
                MainWindow.Instance.UpdateBackgroundInputPointShape((int)args.OldValue, pinID, Brushes.Black);
                MainWindow.Instance.UpdateBackgroundInputPointShape((int)args.NewValue, pinID, Brushes.Red);
            }
            else if(args.Row.Path == "PinID")
            {
                int adapterID = (int)((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("PinID", "AdapterID"));
                MainWindow.Instance.UpdateBackgroundInputPointShape(adapterID, (int)args.OldValue, Brushes.Black);
                MainWindow.Instance.UpdateBackgroundInputPointShape(adapterID, (int)args.NewValue, Brushes.Red);
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
