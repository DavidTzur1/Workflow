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
using System.ComponentModel;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Bars;
using System.Collections.ObjectModel;
using WFBuilder.Models;
using DevExpress.Xpf.PropertyGrid;
using DevExpress.Utils.Serializing;
using DevExpress.Data.Browsing;
using WFBuilder.Adapters;

namespace WFBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public XElement XMLStencils { get; set; }

        public  int NextAdapterID
        {
            get { return (int)GetValue(NextAdapterIDProperty); }
            set { SetValue(NextAdapterIDProperty, value); }
        }
        public static readonly DependencyProperty NextAdapterIDProperty = DependencyProperty.Register("NextAdapterID", typeof(int), typeof(MainWindow), new PropertyMetadata(0, OnPropertyChanged));

        ////////////////Variables/////////////////////////////////////////////
        public int NextVariableID
        {
            get { return (int)GetValue(NextVariableIDProperty); }
            set { SetValue(NextVariableIDProperty, value); }
        }
        public static readonly DependencyProperty NextVariableIDProperty = DependencyProperty.Register("NextVariableID", typeof(int), typeof(MainWindow), new PropertyMetadata(0, OnPropertyChanged));

        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        public ObservableCollection<VariableModel> Variables
        {
            get { return (ObservableCollection<VariableModel>)GetValue(VariablesProperty); }
            set { SetValue(VariablesProperty, value); }
        }
        public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register("Variables", typeof(ObservableCollection<VariableModel>), typeof(MainWindow));

        ////////////////EntryPoints/////////////////////////////////////////////
        public int NextEntryPointID
        {
            get { return (int)GetValue(NextEntryPointIDProperty); }
            set { SetValue(NextEntryPointIDProperty, value); }
        }
        public static readonly DependencyProperty NextEntryPointIDProperty = DependencyProperty.Register("NextEntryPointID", typeof(int), typeof(MainWindow), new PropertyMetadata(0, OnPropertyChanged));
        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        public ObservableCollection<EntryPointModel> EntryPoints
        {
            get { return (ObservableCollection<EntryPointModel>)GetValue(EntryPointsProperty); }
            set { SetValue(EntryPointsProperty, value); }
        }
        public static readonly DependencyProperty EntryPointsProperty = DependencyProperty.Register("EntryPoints", typeof(ObservableCollection<EntryPointModel>), typeof(MainWindow));

        ////////////////BroadcastMessages/////////////////////////////////////////////

        public int NextBroadcastMessageID
        {
            get { return (int)GetValue(NextBroadcastMessageIDProperty); }
            set { SetValue(NextBroadcastMessageIDProperty, value); }
        }
        public static readonly DependencyProperty NextBroadcastMessageIDProperty = DependencyProperty.Register("NextBroadcastMessageID", typeof(int), typeof(MainWindow), new PropertyMetadata(0, OnPropertyChanged));
        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Display (Order =10)]
        public ObservableCollection<BroadcastMessageModel> BroadcastMessages
        {
            get { return (ObservableCollection<BroadcastMessageModel>)GetValue(BroadcastMessagesProperty); }
            set { SetValue(BroadcastMessagesProperty, value); }
        }
        public static readonly DependencyProperty BroadcastMessagesProperty = DependencyProperty.Register("BroadcastMessages", typeof(ObservableCollection<BroadcastMessageModel>), typeof(MainWindow));

        
        ////////////////////////////////////////////////////////////////////////////

        static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(DataContext as MainWindowViewModel).GetVarablesCommand;
            // MessageBox.Show("My message here");
           
        }
        MainWindowViewModel VM = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //diagramControl.DocumentSource = @"..\..\Document.xml";
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

            Variables = new ObservableCollection<VariableModel>() { new VariableModel { Name = "abc", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 10 }, new VariableModel { Name = "def", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 10 } };
            EntryPoints = new ObservableCollection<EntryPointModel>();
            BroadcastMessages = new ObservableCollection<BroadcastMessageModel>();

           


        }

        static MainWindow()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(MainWindow));
        }

        private void PropertyGridControl_ValidateCell(object sender, ValidateCellEventArgs e)
        {
            if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()))
                e.ValidationException = new System.Exception("A cell cannot be empty");
        }

        private void DiagramControl_CustomGetEditableItemPropertiesCacheKey(object sender, DiagramCustomGetEditableItemPropertiesCacheKeyEventArgs e)
        {
            //if (e.Item is DiagramRoot) e.CacheKey = null;
            e.CacheKey = null;
        }

        private void DiagramControl_Loaded(object sender, RoutedEventArgs e)
        {
            RibbonControl ribbon = LayoutHelper.FindElementByType<RibbonControl>(diagramControl);
            // ribbon.ToolbarItems.Remove(ribbon.ToolbarItems.FirstOrDefault(item => ((BarItemLink)item).BarItemName == DefaultBarItemNames.Save));
           // diagramControl.DocumentSource = @"..\..\Documents\Document.xml";

            //var a = GetInputPins(4);
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
                (e.Item as BaseAdapter).Tag = NextAdapterID;
                (e.Item as BaseAdapter).Header = (e.Item as BaseAdapter).Name + NextAdapterID++;
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
                    item.Item.Tag = NextAdapterID;
                    (item.Item as BaseAdapter).Header = (item.Item as BaseAdapter).Name + NextAdapterID++;

                }
            }

        }

        void InitializeStencils()
        {
            XMLStencils = XElement.Load(ConfigurationManager.AppSettings["Stencils"]);
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
                //e.Properties.Add(TypeDescriptor.GetProperties(typeof(MainWindow))["CunterID"]);NextVariableID
                e.Properties.Add(e.CreateProxyProperty("NextAdapterID", item => NextAdapterID, (item, value) => NextAdapterID = value));

                e.Properties.Add(e.CreateProxyProperty("NextVariableID", item => NextVariableID, (item, value) => NextVariableID = value));
                e.Properties.Add(e.CreateProxyProperty("Variables", item => Variables, (item, value) => Variables = value));

                e.Properties.Add(e.CreateProxyProperty("NextEntryPointID", item => NextEntryPointID, (item, value) => NextEntryPointID = value));
                e.Properties.Add(e.CreateProxyProperty("EntryPoints", item => EntryPoints, (item, value) => EntryPoints = value));

                e.Properties.Add(e.CreateProxyProperty("NextBroadcastMessageID", item => NextBroadcastMessageID, (item, value) => NextBroadcastMessageID = value));
                e.Properties.Add(e.CreateProxyProperty("BroadcastMessages", item => BroadcastMessages, (item, value) => BroadcastMessages = value));
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
                (e.Item as BaseAdapter).AddProperties(e);
            }

            ////////////////////////////////////////////////////////////////////////
            else if (e.Item is DiagramRoot)
            {
                e.Properties.Add(e.CreateProxyProperty("NextAdapterID", item => NextAdapterID, (item, value) => NextAdapterID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));

                e.Properties.Add(e.CreateProxyProperty("Variables", item => Variables, (item, value) => Variables = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot",Order=1 } }));
                e.Properties.Add(e.CreateProxyProperty("NextVariableID", item => NextVariableID, (item, value) => NextVariableID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));

                e.Properties.Add(e.CreateProxyProperty("EntryPoints", item => EntryPoints, (item, value) => EntryPoints = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot" ,Order=1} }));
                e.Properties.Add(e.CreateProxyProperty("NextEntryPointID", item => NextEntryPointID, (item, value) => NextEntryPointID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));

                e.Properties.Add(e.CreateProxyProperty("BroadcastMessages", item => BroadcastMessages, (item, value) => BroadcastMessages = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot", Order = 1 }} ));
                e.Properties.Add(e.CreateProxyProperty("NextBroadcastMessageID", item => NextBroadcastMessageID, (item, value) => NextBroadcastMessageID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));
            }
        }
        private void Diagram_QueryConnectionPoints(object sender, DiagramQueryConnectionPointsEventArgs e)
        {
            log.Debug("Diagram_QueryConnectionPoints");

            DiagramShape shape = e.HoveredItem as DiagramShape;
            if (shape != null)
            {
                if (shape.Tag.ToString() == "input")
                {
                    log.Debug("input");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.Begin)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
                else if (shape.Tag.ToString() == "output")
                {
                    log.Debug("output");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.End)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
            }
        }

        public List<AdapterModel> Adapters
        {
            get
            {
                var list = new List<AdapterModel>();
                foreach (var item in diagramControl.Items)
                {
                    if (item is BaseAdapter)
                    {
                        list.Add(new AdapterModel() { AdapterID = int.Parse(item.Tag.ToString()),AdapterName=(item as BaseAdapter).Header });
                    }
                }
                return list;
            }
        }

        public  List<PinModel> GetInputPins(int adapterID )
        {
            var list = new List<PinModel>();
            var adapter = diagramControl.Items.Where(item => item.Tag?.ToString() == adapterID.ToString()).FirstOrDefault() as BaseAdapter;
            var inputPanel = adapter.Items.Where(item => item.Tag?.ToString() == "InputPanel").FirstOrDefault() as DiagramList;
            if(inputPanel != null)
            {
                foreach(var item in inputPanel.Items)
                {
                    if(item.Tag?.ToString() == "DiagramContainer")
                    {
                        var lineShape = (item as DiagramContainer).Items.Where(shape => shape.Tag?.ToString() == "line").FirstOrDefault() as DiagramShape;
                        var labelShape = (item as DiagramContainer).Items.Where(shape => shape.Tag?.ToString() == "label").FirstOrDefault() as DiagramShape;
                        list.Add(new PinModel() { PinID = int.Parse(lineShape.Content),PinName= labelShape.Content });
                    }
                }
            }
            return list;
        }
        
    }
}
    



