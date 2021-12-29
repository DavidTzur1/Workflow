using DevExpress.Xpf.Diagram;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            HiddenEditor += PropertyGridControlEx_HiddenEditor;



        }

        private void PropertyGridControlEx_HiddenEditor(object sender, PropertyGridEditorEventArgs args)
        {
            _currentEditor = null;
        }

        private IBaseEdit _currentEditor;
        private void PropertyGridControlEx_ShownEditor(object sender, PropertyGridEditorEventArgs args)
        {
            Debug.WriteLine($"EditorType={args.Editor.GetType()} FullPath={args.Row.FullPath}" );
            _currentEditor = args.Editor;
            if (args.Editor is ComboBoxEdit && args.Row.Path == "PinID")
            {
                var rowValue = ((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("PinID", "AdapterID"));
                if (rowValue != null)
                    ((ComboBoxEdit)args.Editor).ItemsSource = MainWindow.Instance.GetInputPins((int)rowValue);
            }

            if (args.Editor is ComboBoxEdit)
            {
                if ((args.Editor as ComboBoxEdit).Tag?.ToString() == "ValTypeEditor" )
                {
                    var rowValue = ((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("ValType", "AdapterIDs"));
                    if (rowValue != null)
                        ((ComboBoxEdit)args.Editor).IsEnabled = (((ObservableCollection<int>)rowValue).Count > 0)?false:true;
                }
                if ((args.Editor as ComboBoxEdit).Tag?.ToString() == "LevelScopeEditor")
                {
                    var rowValue = ((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("LevelScope", "AdapterIDs"));
                    if (rowValue != null)
                        ((ComboBoxEdit)args.Editor).IsEnabled = (((ObservableCollection<int>)rowValue).Count > 0) ? false : true;
                }

            }
            if(args.Editor is InplaceBaseEdit)
            {
                if(args.Row.FullPath.StartsWith("Variables.[") && args.Row.Path=="Name")
                {
                    var rowValue = ((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath(args.Row.FullPath.Replace("Name", "AdapterIDs"));
                    if (rowValue != null)
                        ((InplaceBaseEdit)args.Editor).IsEnabled = (((ObservableCollection<int>)rowValue).Count > 0) ? false : true;
                }              
            }
        }

        private void PropertyGridControlEx_CellValueChanged(object sender, CellValueChangedEventArgs args)
        {
            /////////////Update the BackgroundInputPoint
            //log.Debug($" args.Row.FullPath={args.Row.FullPath} CellValueChanged OldValue={args.OldValue} NewValue={args.NewValue}");
            //log.Debug("PropertyGridControlEx_CellValueChanged");
            Debug.WriteLine($" args.Row.FullPath={args.Row.FullPath} CellValueChanged OldValue={args.OldValue} NewValue={args.NewValue}");
            if (args.Row.Path == "AdapterID")
            {            
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

            else if (_currentEditor is PopupBaseEdit)
            {
                if ((_currentEditor as PopupBaseEdit).Tag?.ToString() == "VariablesIntEditor")
                {
                    int adapterID = (int)((PropertyGridControl)args.OriginalSource).GetRowValueByRowPath("Adapter Id");
                    if (args.NewValue != null)
                    {
                        if (args.NewValue.ToString().StartsWith("@") || args.NewValue.ToString().StartsWith("::"))
                        {
                            var newVal = args.NewValue.ToString().TrimStart('@');
                            MainWindow.Instance.Variables.Where(x => x.Name == newVal).FirstOrDefault()?.AdapterIDs.Add(adapterID);
                        }
                    }
                    if ((args.OldValue != null))
                    {
                        if (args.OldValue.ToString().StartsWith("@") || args.NewValue.ToString().StartsWith("::"))
                        {
                            var oldVal = args.OldValue.ToString().TrimStart('@');
                            MainWindow.Instance.Variables.Where(x => x.Name == oldVal).FirstOrDefault()?.AdapterIDs.Remove(adapterID);
                        }

                    }
                }
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
