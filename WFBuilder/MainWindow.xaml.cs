using DevExpress.Diagram.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WFBuilder.Activities;
using WFBuilder.Shapes;

namespace WFBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public XElement XMLStencils { get; set; }
        static public XElement XMLActivities { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            RemoveAllDefaultStencils();
            InitializeStencils();

            diagramControl.QueryConnectionPoints += Diagram_QueryConnectionPoints;
            diagramControl.CustomGetEditableItemProperties += Diagram_CustomGetEditableItemProperties;
           
        }

       
        void InitializeStencils()
        {
            XMLStencils = XElement.Load(ConfigurationManager.AppSettings["Stencils"]);
            XMLActivities = XElement.Load(ConfigurationManager.AppSettings["Activities"]);

            List<string> stencilCollection = new List<string>();
            foreach (XElement stencil in XMLStencils.Element("Stencils").Elements("Stencil"))
            {
                string id = stencil.Attribute("id").Value;
                string name = stencil.Attribute("name").Value;
                stencilCollection.Add(id);
                var diagramStencil = new DevExpress.Diagram.Core.DiagramStencil(id, name);
                foreach (XElement activity in stencil.Element("Activities").Elements("Activity"))
                {
                    int pinsQtyIn = int.Parse(activity.Attribute("PinsQtyIn").Value);
                    int pinsQtyOut = int.Parse(activity.Attribute("PinsQtyOut").Value);
                    FactoryItemTool containerTool = new FactoryItemTool(activity.Attribute("Type").Value, () => activity.Attribute("Type").Value, d => CreateDecoderContianer(activity.Attribute("Namespace").Value, activity.Attribute("Type").Value, pinsQtyIn, pinsQtyOut));
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

       

        private void Diagram_CustomGetEditableItemProperties(object sender, DiagramCustomGetEditableItemPropertiesEventArgs e)
        {

            log.Debug("CustomGetEditableItemProperties " + e.Item.Name);
            if (e.Item.GetType().IsSubclassOf(typeof(DiagramContainerBase)))
            {
                //////////////Remove all the standard properties //////////////////////
                var iterator = e.Properties.GetEnumerator();
                while (iterator.MoveNext())
                {
                    e.Properties.RemoveAt(0);
                }


                /////////////add your custom properties//////////////////////////////////////
                XElement activity = XMLActivities.Element("Activities").Elements("Activity").FirstOrDefault(item => item.Attribute("Type").Value == e.Item.Name);
                foreach (XElement property in activity.Element("Properties").Elements("Property"))
                {
                    PropertyDescriptor infoPropertyDescriptor = TypeDescriptor.GetProperties(Type.GetType(activity.Attribute("DiagramFullName").Value))[property.Attribute("name").Value];
                    e.Properties.Add(infoPropertyDescriptor);
                }

                //////////////////////////////////////////////////////////////////////////////
            }
        }

        private void Diagram_QueryConnectionPoints(object sender, DiagramQueryConnectionPointsEventArgs e)
        {
            log.Debug("Diagram_QueryConnectionPoints");

            DiagramShape shape = e.HoveredItem as DiagramShape;
            if (shape != null)
            {
                if (string.Equals(shape.Tag, "input"))
                {
                    log.Debug("input");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.Begin)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
                else if (string.Equals(shape.Tag, "output"))
                {
                    log.Debug("output");
                    e.ItemConnectionBorderState = ConnectionElementState.Hidden;
                    if (e.ConnectorPointType == ConnectorPointType.End)
                        e.ItemConnectionPointStates.ToList().ForEach(cp => cp.State = ConnectionElementState.Disabled);
                }
            }
        }



            public DiagramContainer CreateDecoderContianer(string path, string typeName, int pinsQtyIn, int pinsQtyOut, int Width = 120, int Height = 230)
        {
            log.Debug("CreateDecoderContianer");
            string fullName = $"{path}.{typeName}";
            ObjectHandle handle = Activator.CreateInstance("WFBuilder", fullName);
            DiagramContainerBase decoderContainer = (DiagramContainerBase)handle.Unwrap();

            log.Debug($"PinsIn={decoderContainer.PinsIn}");

            int spacing = Height / (pinsQtyIn + 1);

            for (int i = 1; i <= pinsQtyIn; ++i)
            {
                log.Debug(spacing * i);
                decoderContainer.Items.Add(InputPointShape.Create(new Point(0, spacing * i)));
                decoderContainer.Items.Add(LabelShape.Create(new Point(12, spacing * i - 10), "in" + i, Sides.Left));
            }

            spacing = Height / (pinsQtyOut + 1);

            for (int i = 1; i <= pinsQtyOut; i++)
            {
                decoderContainer.Items.Add(OutputPointShape.Create(new Point(Width - 10, spacing * i)));
                decoderContainer.Items.Add(LabelShape.Create(new Point(60, spacing * i - 10), "out" + i, Sides.Right));
            }


            return decoderContainer;
        }
       

    }
}

