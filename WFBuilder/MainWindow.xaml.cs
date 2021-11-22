using DevExpress.Diagram.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WFBuilder.Shapes;
using WFBuilder.DiagramContainers;
using System.ComponentModel;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Bars;
using System.Collections.ObjectModel;
using WFBuilder.Models;
using DevExpress.Xpf.PropertyGrid;

namespace WFBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public XElement XMLStencils { get; set; }
        static public XElement XMLDiagramContainers { get; set; }

        public int CunterID
        {
            get { return (int)GetValue(CunterIDProperty); }
            set { SetValue(CunterIDProperty, value); }
        }
        public static readonly DependencyProperty CunterIDProperty = DependencyProperty.Register("CunterID", typeof(int), typeof(MainWindow));
        private void BarButtonItem_ItemDoubleClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MessageBox.Show("My message here");
        }

       

        public MainWindow()
        {
            InitializeComponent();
            CunterID = 0;
            RemoveAllDefaultStencils();
            InitializeStencils();

            diagramControl.QueryConnectionPoints += Diagram_QueryConnectionPoints;
            diagramControl.CustomGetEditableItemProperties += diagram_CustomGetEditableItemProperties;
            diagramControl.CustomGetEditableItemPropertiesCacheKey += DiagramControl_CustomGetEditableItemPropertiesCacheKey;
            diagramControl.CustomGetSerializableItemProperties += diagram_CustomGetSerializableItemProperties;
            diagramControl.AddingNewItem += DiagramControl_AddingNewItem;
            diagramControl.ItemsPasting += DiagramControl_ItemsPasting;
            diagramControl.ItemInitializing += Diagram_ItemInitializing;
            diagramControl.Loaded += DiagramControl_Loaded;

            DSVariables dSVariables = new DSVariables();
            diagramControl.Items.Add(dSVariables);

            



        }

        private void PropertyGridControl_ValidateCell(object sender, ValidateCellEventArgs e)
        {
            if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()))
                e.ValidationException = new System.Exception("A cell cannot be empty");
        }

        private void DiagramControl_CustomGetEditableItemPropertiesCacheKey(object sender, DiagramCustomGetEditableItemPropertiesCacheKeyEventArgs e)
        {
            if (e.Item is DiagramRoot) e.CacheKey = null;
        }

        private void DiagramControl_Loaded(object sender, RoutedEventArgs e)
        {
            RibbonControl ribbon = LayoutHelper.FindElementByType<RibbonControl>(diagramControl);
           // ribbon.ToolbarItems.Remove(ribbon.ToolbarItems.FirstOrDefault(item => ((BarItemLink)item).BarItemName == DefaultBarItemNames.Save));
        }

        

        private void Diagram_ItemInitializing(object sender, DiagramItemInitializingEventArgs e)
        {
            DiagramConnector connector = e.Item as DiagramConnector;
            if (connector != null)
                connector.Type = ConnectorType.Straight;
        }

        private void DiagramControl_AddingNewItem(object sender, DiagramAddingNewItemEventArgs e)
        {
            //Add Id and Header to adapter
            if (e.Item is BaseAdapter)
            {
                (e.Item as BaseAdapter).Tag = CunterID;
                (e.Item as BaseAdapter).Header = (e.Item as BaseAdapter).Name + CunterID++;
            }

            //Prohibit adding connectors without end items
            DiagramConnector connector = e.Item as DiagramConnector;
            if (connector != null && (connector.BeginItem == null || connector.EndItem == null))
                e.Cancel = true;
        }

        private void DiagramControl_ItemsPasting(object sender, DiagramItemsPastingEventArgs e)
        {
            foreach (var item in e.Items)
            {
                if (item.Item is BaseAdapter)
                {
                    item.Item.Tag = CunterID;
                    (item.Item as BaseAdapter).Header = (item.Item as BaseAdapter).Name + CunterID++;

                }
            }

        }

        void InitializeStencils()
        {
            XMLStencils = XElement.Load(ConfigurationManager.AppSettings["Stencils"]);
            XMLDiagramContainers = XElement.Load(ConfigurationManager.AppSettings["DiagramContainers"]);

            List<string> stencilCollection = new List<string>();
            foreach (XElement stencil in XMLStencils.Element("Stencils").Elements("Stencil"))
            {
                string id = stencil.Attribute("id").Value;
                string name = stencil.Attribute("name").Value;
                stencilCollection.Add(id);
                var diagramStencil = new DevExpress.Diagram.Core.DiagramStencil(id, name);
                foreach (XElement activity in stencil.Element("Activities").Elements("Activity"))
                {
                    FactoryItemTool containerTool = new FactoryItemTool(activity.Attribute("Type").Value, () => activity.Attribute("Type").Value, d => BaseAdapter.Create(activity.Attribute("Namespace").Value, activity.Attribute("Type").Value));

                    diagramStencil.RegisterTool(containerTool);
                }

                DevExpress.Diagram.Core.DiagramToolboxRegistrator.RegisterStencil(diagramStencil);
            }
            diagramControl.SelectedStencils = new StencilCollection(stencilCollection);
        }

        private void RemoveAllDefaultStencils()
        {
            List<DiagramStencil> Stencils = DiagramToolboxRegistrator.Stencils.ToList<DiagramStencil>();
            Stencils.ForEach(s => DiagramToolboxRegistrator.UnregisterStencil(s));
        }

       

        private void diagram_CustomGetSerializableItemProperties(object sender, DevExpress.Xpf.Diagram.DiagramCustomGetSerializableItemPropertiesEventArgs e)
        {
            if (e.ItemType == typeof(DiagramRoot))
            {
                e.Properties.Add(e.CreateProxyProperty("CunterID", item => CunterID, (item, value) => CunterID = value));
            }
        }

        private void diagram_CustomGetEditableItemProperties(object sender, DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //////////////Remove all the standard properties //////////////////////
            var iterator = e.Properties.GetEnumerator();
            while (iterator.MoveNext())
            {
                e.Properties.RemoveAt(0);
            }

            //////////////////////////////////////////////////////////////////////
            if (e.Item is BaseAdapter)
            {

                ///////////////////Add Adapter properties //////////////////////
                (e.Item as BaseAdapter).AddProperties(e);
            }
            else if (e.Item is DSVariables)
            {
                ///////////////Add Adapter properties //////////////////////
                (e.Item as DSVariables).AddProperties(e);
            }
            else if(e.Item is DiagramRoot)
            {
                e.Properties.Add(TypeDescriptor.GetProperties(typeof(MainWindow))["CunterID"]);
            }
        }
           



        private void Diagram_CustomGetEditableItemProperties(object sender, DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            try
            {

                log.Debug("CustomGetEditableItemProperties " + e.Item.Name);
                if (e.Item.GetType().IsSubclassOf(typeof(DiagramContainerEx)))
                {
                    //////////////Remove all the standard properties //////////////////////
                    var iterator = e.Properties.GetEnumerator();
                    while (iterator.MoveNext())
                    {
                        e.Properties.RemoveAt(0);
                    }


                    var items = e.Item.Controller.Item.NestedItems;
                    int i = 1;
                    int inIndex = 1;
                    int outIndex = 33;
                    foreach (var item in items)
                    {
                        log.Debug("Tag=" + item.Tag?.ToString());

                        if (item is InputPointShape)
                        {
                            e.Properties.Add(e.CreateProxyProperty("in" + inIndex++, pin => ((InputPointShape)item).PinID, (pin, value) => ((InputPointShape)item).PinID = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins In" }, new ReadOnlyAttribute(true) }));
                        }
                        if (item is OutputPointShape)
                        {
                            e.Properties.Add(e.CreateProxyProperty("out" + outIndex++, pin => ((OutputPointShape)item).PinID, (pin, value) => ((OutputPointShape)item).PinID = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins Out" }, new ReadOnlyAttribute(true) }));
                        }

                    }


                    /////////////Add Base custom properties//////////////////////////////////////
                    PropertyDescriptor infoPropertyDescriptor = TypeDescriptor.GetProperties(Type.GetType("WFBuilder.DiagramContainers.DiagramContainerEx"))["ID"];
                    e.Properties.Add(infoPropertyDescriptor);
                    infoPropertyDescriptor = TypeDescriptor.GetProperties(Type.GetType("WFBuilder.DiagramContainers.DiagramContainerEx"))["NameID"];
                    e.Properties.Add(infoPropertyDescriptor);
                    //e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.DiagramContainers.DiagramContainerEx"))["InputPins"]) ;
                    //e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.DiagramContainers.DiagramContainerEx"))["OutputPins"]);

                    /////////////add your custom properties//////////////////////////////////////
                    XElement diagramContainer = XMLDiagramContainers.Element("DiagramContainers").Elements("DiagramContainer").FirstOrDefault(item => item.Attribute("Type").Value == e.Item.Name);
                    foreach (XElement property in diagramContainer.Element("Properties").Elements("Property"))
                    {
                        infoPropertyDescriptor = TypeDescriptor.GetProperties(Type.GetType(diagramContainer.Attribute("DiagramFullName").Value))[property.Attribute("name").Value];
                        e.Properties.Add(infoPropertyDescriptor);
                    }

                    

                    //////////////////////////////////////////////////////////////////////////////
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

        private void Diagram_QueryConnectionPoints(object sender, DiagramQueryConnectionPointsEventArgs e)
        {
            log.Debug("Diagram_QueryConnectionPoints");

            DiagramShape shape = e.HoveredItem as DiagramShape;
            if (shape != null)
            {
                if (shape.Tag.ToString()== "input")
                {
                    log.Debug("input");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.Begin)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
                else if (shape.Tag.ToString() =="output")
                {
                    log.Debug("output");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.End)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
            }
        }
    }
}


