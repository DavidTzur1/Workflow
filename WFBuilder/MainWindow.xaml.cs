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
using DevExpress.Xpf.Editors;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.IO;

namespace WFBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow, INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MainWindow Instance;
        public int CurrentAdapterID { get; set; } = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

            static public XElement XMLStencils { get; set; }

        public int NextAdapterID
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
        public static readonly DependencyProperty NextEntryPointIDProperty = DependencyProperty.Register("NextEntryPointID", typeof(int), typeof(MainWindow));
        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        public ObservableCollection<EntryPointModel> EntryPoints
        {
            get { return (ObservableCollection<EntryPointModel>)GetValue(EntryPointsProperty); }
            set 
            { 
                SetValue(EntryPointsProperty, value);
                //MessageBox.Show("Set EntryPoints");
            }
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
        [Display(Order = 10)]
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

        static void OnPropertyChanged1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(DataContext as MainWindowViewModel).GetVarablesCommand;
            MessageBox.Show($"OnPropertyChanged1 e= {e.Property} Old={e.OldValue} New={e.NewValue}");

        }
       // MainWindowViewModel VM = new MainWindowViewModel();
        
        public MainWindow()
        {
            //MessageBox.Show("MainWindow");
            InitializeComponent();
            DataContext = this;
            //diagramControl.DocumentSource = @"..\..\Document.xml";
            RemoveAllDefaultStencils();
            InitializeStencils();
            Instance = this;

            diagramControl.QueryConnectionPoints += Diagram_QueryConnectionPoints;
            diagramControl.CustomGetEditableItemProperties += diagram_CustomGetEditableItemProperties;
            diagramControl.CustomGetEditableItemPropertiesCacheKey += DiagramControl_CustomGetEditableItemPropertiesCacheKey;
            diagramControl.CustomGetSerializableItemProperties += diagram_CustomGetSerializableItemProperties;
            diagramControl.AddingNewItem += DiagramControl_AddingNewItem;
            diagramControl.ItemsPasting += DiagramControl_ItemsPasting;
            diagramControl.ItemInitializing += Diagram_ItemInitializing;
            diagramControl.Loaded += DiagramControl_Loaded;
            diagramControl.CustomLoadDocument += DiagramControl_CustomLoadDocument;
            diagramControl.DocumentLoaded += DiagramControl_DocumentLoaded;
            
           
            Variables = new ObservableCollection<VariableModel>() 
            { 
                new VariableModel { Name = "Int1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 10 },
                new VariableModel { Name = "Int2", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 20 },
                new VariableModel { Name = "Str1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.String, Val = "ValSrr1" },
                new VariableModel { Name = "Str2", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.String, Val = "ValStr2" },
                new VariableModel { Name = "Date1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.DateTime, Val = DateTime.Now.ToString() },
                new VariableModel { Name = "Dou1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Double, Val = 3.5 },
                new VariableModel { Name = "Bool1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Boolean, Val = false },
                new VariableModel { Name = "Obj1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Object, Val = "" },
                new VariableModel { Name = "Cur", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Currency, Val = 5.30 },
            };

            Variables.CollectionChanged += Variables_CollectionChanged;
            

            EntryPoints = new ObservableCollection<EntryPointModel>() ;
            EntryPoints.CollectionChanged += EntryPoints_CollectionChanged;

            BroadcastMessages = new ObservableCollection<BroadcastMessageModel>()
            {
                new BroadcastMessageModel{BroadcastMessageID=0,BroadcastMessageName="bk"},
                new BroadcastMessageModel{BroadcastMessageID=1,BroadcastMessageName="stop"}
            };
            BroadcastMessages.CollectionChanged += BroadcastMessages_CollectionChanged;



        }

       

        private void Variables_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // MessageBox.Show($"Variables_CollectionChanged Action= { e.Action}");
        }

        private void DiagramControl_DocumentLoaded(object sender, DiagramDocumentLoadedEventArgs e)
        {
            EntryPoints.CollectionChanged += EntryPoints_CollectionChanged;
            BroadcastMessages.CollectionChanged += BroadcastMessages_CollectionChanged;
            foreach(BroadcastMessageModel model in BroadcastMessages)
            {
                model.BrodcastTarges.CollectionChanged += model.BrodcastTarges_CollectionChanged;
            }
            
            

        }

        private void EntryPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //MessageBox.Show("EntryPoints_CollectionChanged");

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (EntryPointModel item in e.OldItems)
                {
                    MainWindow.Instance.UpdateBackgroundInputPointShape(item.AdapterID, item.PinID, Brushes.Black);
                }
            }
        }
        private void BroadcastMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (BroadcastMessageModel model in e.OldItems)
                {
                    foreach(BrodcastTargetModel item in model.BrodcastTarges)
                    MainWindow.Instance.UpdateBackgroundInputPointShape(item.AdapterID, item.PinID, Brushes.Black);
                }
            }
        }

        private void DiagramControl_CustomLoadDocument(object sender, DiagramCustomLoadDocumentEventArgs e)
        {

            if (e.DocumentSource == null)
            {

                NextAdapterID = 0;
                NextBroadcastMessageID = 0;
                NextEntryPointID = 0;
                NextVariableID = 0;
                Variables = new ObservableCollection<VariableModel>()
                {
                    new VariableModel { Name = "Int1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 10 },
                    new VariableModel { Name = "Int2", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.Integer, Val = 10 },
                    new VariableModel { Name = "Str1", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.String, Val = "ValSrr1" },
                    new VariableModel { Name = "Str2", LevelScope = LevelScopeType.Local, ValType = ValidationDataTypeEx.String, Val = "ValStr2" }
                };

                EntryPoints.Clear();
                BroadcastMessages = new ObservableCollection<BroadcastMessageModel>();

               



            }
            
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
            //diagramControl.DocumentSource = @"..\..\Documents\Document.xml";
           // MessageBox.Show("Loaded");

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
               // (e.Item as BaseAdapter).Tag = NextAdapterID;
                (e.Item as BaseAdapter).AdapterID = NextAdapterID;
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
                    //item.Item.Tag = NextAdapterID;
                    (item.Item as BaseAdapter).AdapterID = NextAdapterID;
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
                foreach (XElement adapter in stencil.Element("Adapters").Elements("Adapter"))
                {
                    FactoryItemTool containerTool = new FactoryItemTool(adapter.Attribute("Type").Value, () => adapter.Attribute("Name").Value, d => BaseAdapter.Create(adapter.Attribute("Namespace").Value, adapter.Attribute("Type").Value, adapter.Attribute("Name").Value));

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

                e.Properties.Add(e.CreateProxyProperty("Variables", item => Variables, (item, value) => Variables = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot", Order = 1 }, new PropertyGridEditorAttribute() { TemplateKey = "VariablesEditor" } }));
                e.Properties.Add(e.CreateProxyProperty("NextVariableID", item => NextVariableID, (item, value) => NextVariableID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));

                e.Properties.Add(e.CreateProxyProperty("EntryPoints", item => EntryPoints, (item, value) => EntryPoints = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot", Order = 1 } }));
                e.Properties.Add(e.CreateProxyProperty("NextEntryPointID", item => NextEntryPointID, (item, value) => NextEntryPointID = value, new Attribute[] { new DisplayAttribute() { GroupName = "IDs" }, new ReadOnlyAttribute(true) }));

                e.Properties.Add(e.CreateProxyProperty("BroadcastMessages", item => BroadcastMessages, (item, value) => BroadcastMessages = value, new Attribute[] { new DisplayAttribute() { GroupName = "DiagramRoot", Order = 1 } }));
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
                var list = new List<AdapterModel>() { new AdapterModel { AdapterID = -1, AdapterName = "<Empty>" } };
                foreach (var item in diagramControl.Items)
                {
                    if (item is BaseAdapter)
                    {
                        list.Add(new AdapterModel() { AdapterID = (item as BaseAdapter).AdapterID, AdapterName = (item as BaseAdapter).Header });
                    }
                }
                return list;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////

        private List<PinModel> _inputPins;
        public List<PinModel> InputPins
        {
            get
            {
                if (_inputPins == null) _inputPins = new List<PinModel>() { new PinModel() { PinID = -1, PinName = "<Empty>" } };
                return _inputPins;
            }
            set
            {
                _inputPins = value;
                NotifyPropertyChanged("InputPins");
            }
        }

        public List<PinModel> GetInputPins(int adapterID)
        {
            var list = new List<PinModel>() { new PinModel() { PinID = -1, PinName = "<Empty>" } };
            if (adapterID == -1) return list;
            var adapter = diagramControl.Items.Where(item  => (item as BaseAdapter).AdapterID == adapterID).FirstOrDefault() as BaseAdapter;
            var inputPanel = adapter.Items.Where(item => item.Tag?.ToString() == "InputPanel").FirstOrDefault() as DiagramList;
            if (inputPanel != null)
            {
                foreach (var item in inputPanel.Items)
                {
                    if (item.Tag?.ToString() == "DiagramContainer")
                    {
                        var lineShape = (item as DiagramContainer).Items.Where(shape => shape.Tag?.ToString() == "line").FirstOrDefault() as DiagramShape;
                        var labelShape = (item as DiagramContainer).Items.Where(shape => shape.Tag?.ToString() == "label").FirstOrDefault() as DiagramShape;
                        list.Add(new PinModel() { PinID = int.Parse(lineShape.Content), PinName = labelShape.Content });
                    }
                }
            }
            return list;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateBackgroundInputPointShape(int adapterID, int pinID, Brush brush)
        {
            if (adapterID == -1 || pinID == -1) return;
            var adapter = diagramControl.Items.Where(item => (item as BaseAdapter).AdapterID == adapterID).FirstOrDefault() as BaseAdapter;
            if (adapter == null) return;
            var inputPanel = adapter.Items.Where(item => item.Tag?.ToString() == "InputPanel").FirstOrDefault() as DiagramList;
            var diagramContainer = inputPanel.Items.Where(item => ((item as DiagramContainer).Items.Where(shape => shape.Tag?.ToString() == "line" && (shape as DiagramShape).Content == pinID.ToString())).Count() > 0).FirstOrDefault() as DiagramContainer;
            if(diagramContainer != null)
            {
                DiagramShape shape = diagramContainer.Items.Where(item => item.Tag?.ToString() == "input").FirstOrDefault() as DiagramShape;
                shape.Background = brush;
            }
        }

        ///////////////Varabels//////////////////////////////////////////////////////////////////////////

        public List<VariableModel> VariablesGeneric(string valTypes)
        {

            List<VariableModel> list = new List<VariableModel>();
            foreach (var item in Variables)
            {
                if(valTypes.Contains(Enum.GetName(typeof(ValidationDataTypeEx),item.ValType)) || valTypes=="All")
                {
                    list.Add(new VariableModel { Name = (item.LevelScope == LevelScopeType.Local ? $"@{item.Name}" : $"::{item.Name}"), VariableID = item.VariableID });
                }
               
            }

            return list;
        }

        ///////////////BroadcastMessage//////////////////////////////////////////////////////////////////////////
        public List<string> BroadcastMessageNames
        {
            get
            {

                return BroadcastMessages.Select(x => x.BroadcastMessageName).ToList();
            }
            
        }


    }
}

    



