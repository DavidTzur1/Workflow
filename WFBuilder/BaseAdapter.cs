using DevExpress.Diagram.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;
using System.Runtime.Remoting;
using System.ComponentModel;
using WFBuilder.Models;

namespace WFBuilder
{
    public class Pin
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public  class BaseAdapter : DiagramList
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public List<Pin> PinsIn = new List<Pin>();
        public List<Pin> PinsOut = new List<Pin>();


        public BaseAdapter()
        {
            Tag = "Adapter";
            ItemsCanSelect = false;
            ItemsCanMove = false;
            DragMode = ContainerDragMode.ByAnyPoint;
            ShowHeader = true;
            CanAttachConnectorBeginPoint = false;
            CanAttachConnectorEndPoint = false;
            Orientation = Orientation.Horizontal;
            Stroke = Brushes.Transparent;
            CanAddItems = false;
            
        }

        static BaseAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(BaseAdapter));
        }

        public virtual void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            e.Properties.Add(e.CreateProxyProperty("Adapter Id", adapter => e.Item.Tag, (adapter, value) => e.Item.Tag = value, new Attribute[] { new DisplayAttribute() { GroupName = "Adapter" }, new ReadOnlyAttribute(true) }));
            e.Properties.Add(e.CreateProxyProperty("Adapter Name", adapter => (e.Item as BaseAdapter).Header, (adapter, value) => (e.Item as BaseAdapter).Header = value, new Attribute[] { new DisplayAttribute() { GroupName = "Adapter" } }));

            foreach (var item in (e.Item as BaseAdapter).Items)
            {

                if (item.Tag?.ToString() == "InputPanel")
                {
                    foreach (var input in (item as DiagramList).Items)
                    {
                        var labelIn = (input as DiagramContainer).Items.First(itm => itm.Tag?.ToString() == "label") as DiagramShape;
                        var lineIn = (input as DiagramContainer).Items.First(itm => itm.Tag?.ToString() == "line") as DiagramShape;

                        e.Properties.Add(e.CreateProxyProperty(labelIn.Content, pin => lineIn.Content, (pin, value) => lineIn.Content = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins In" }, new ReadOnlyAttribute(true) }));
                    }
                }

                if (item.Tag?.ToString() == "OutputPanel")
                {
                    foreach (var input in (item as DiagramList).Items)
                    {
                        var labelOut = (input as DiagramContainer).Items.First(itm => itm.Tag?.ToString() == "label") as DiagramShape;
                        var lineOut = (input as DiagramContainer).Items.First(itm => itm.Tag?.ToString() == "line") as DiagramShape;

                        e.Properties.Add(e.CreateProxyProperty(labelOut.Content, pin => lineOut.Content, (pin, value) => lineOut.Content = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins Out" }, new ReadOnlyAttribute(true) }));
                    }
                }
            }
        }

        public static DiagramContainer Create(string path,string typeName)
        {
            string fullName = $"{path}.{typeName}";
            ObjectHandle handle = Activator.CreateInstance("WFBuilder", fullName);
            BaseAdapter adapter = (BaseAdapter)handle.Unwrap();
            
            log.Debug($"Create {adapter.Name} Adapter");

            ////////////Create Pins In ///////////////////////////////////////////////////////////
            
            DiagramList inputsPanel = new DiagramList()
            {
                Tag = "InputPanel",
                ItemsCanSelect = false,
                ItemsCanMove = false,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Width = adapter.Width * 0.275,
                Orientation = Orientation.Vertical,
                Stroke = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                CanAddItems = false,

            };

            

            foreach (var item in adapter.PinsIn)
            {
                DiagramContainer input = CreateDiagramContainer(inputsPanel.Width);
                input.Items.Add(CreateLabelShape(new Point(12, 10), item.name, Sides.Top));
                input.Items.Add(CreateInputPointShape(new Point(0, 25)));
                input.Items.Add(CreateLineShape(new Point(10, 30),item.id.ToString()));
                inputsPanel.Items.Add(input);
                input.SizeChanged += input_SizeChanged;
            }



            ///////////Create Pins Out/////////////////////////////////////////////////////////////

            DiagramList outputsPanel = new DiagramList()
            {
                Tag = "OutputPanel",
                ItemsCanSelect = false,
                ItemsCanMove = false,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Width = adapter.Width * 0.275,
                Orientation = Orientation.Vertical,
                Stroke = Brushes.Transparent,
                CanAddItems = false,

            };

          
            foreach (var item in adapter.PinsOut)
            {
                DiagramContainer output = CreateDiagramContainer(outputsPanel.Width);
                output.Items.Add(CreateLineShape(new Point(0, 30),item.id.ToString(), Sides.Right));
                output.Items.Add(CreateOutputPointShape(new Point(outputsPanel.Width - 10, 25)));
                output.Items.Add(CreateLabelShape(new Point(outputsPanel.Width - 30, 10), item.name, Sides.Top));
                outputsPanel.Items.Add(output);
                output.SizeChanged += output_SizeChanged;

            }

            ///////////Create Image/////////////////////////////////////////////////////////////

            DiagramList imagePanel = new DiagramList()
            {
                ItemsCanSelect = false,
                ItemsCanMove = false,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Width = adapter.Width * 0.45,
                Orientation = Orientation.Vertical,
                Stroke = Brushes.Transparent,
                CanAddItems = false,
            };

            var imageShape = new DiagramImage()

            {
                Position = new Point(0, 0),
                StretchMode = StretchMode.Uniform,
                Background = Brushes.Gray,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false
            };
            imageShape.Image = new BitmapImage(new Uri("../../Images/Globe.png", UriKind.Relative));
            imagePanel.Items.Add(imageShape);

            ///////////////////Add all Panels to adapter///////////////////////////////////////

            adapter.Items.Add(inputsPanel);
            adapter.Items.Add(imagePanel);
            adapter.Items.Add(outputsPanel);

            return adapter;

        }

        private static void input_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var shapes = ((DiagramContainer)e.Source).Items
                .Where(itm => itm.Tag?.ToString() == "input" || itm.Tag?.ToString() == "line" || itm.Tag?.ToString() == "label")
                .ToList();

            foreach (var shape in shapes)
            {
                var verticalOffset = e.NewSize.Height / 2;
                var shapeOffset = shape.ActualHeight / 2;
                shape.Position = shape.Tag.ToString() != "label" ? new Point(shape.Position.X, verticalOffset - shapeOffset) : new Point(shape.Position.X, verticalOffset - shapeOffset - 10);
            }
        }

        private static void output_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var shapes = ((DiagramContainer)e.Source).Items
                .Where(itm => itm.Tag?.ToString() == "output" || itm.Tag?.ToString() == "line" || itm.Tag?.ToString() == "label")
                .ToList();

            foreach (var shape in shapes)
            {
                var verticalOffset = e.NewSize.Height / 2;
                var shapeOffset = shape.ActualHeight / 2;
                shape.Position = shape.Tag.ToString() != "label" ? new Point(shape.Position.X, verticalOffset - shapeOffset) : new Point(shape.Position.X, verticalOffset - shapeOffset - 10);
            }
        }

        static DiagramContainer CreateDiagramContainer(double width)
        {
            return new DiagramContainer()
            {
                //Stroke = Brushes.Red,
                Tag = "DiagramContainer",
                ItemsCanSelect = false,
                ItemsCanMove = false,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Width = width,
                Height = 40,
                
                

            };
        }

        static DiagramShape CreateInputPointShape(Point position)
        {
            return new DiagramShape()
            {
                Tag = "input",
                Anchors = Sides.Left,
                Shape = BasicShapes.Ellipse,
                Position = position,
                MinWidth = 10,
                MinHeight = 10,
                ConnectionPoints = new DiagramPointCollection(new Point[] { new Point(0, 0.5) }),
                Background = Brushes.Black
            };
        }

        static DiagramShape CreateOutputPointShape(Point position)
        {
            return new DiagramShape()
            {
                Tag = "output",
                Anchors = Sides.Right,
                Shape = BasicShapes.Ellipse,
                Position = position,
                MinWidth = 10,
                MinHeight = 10,
                ConnectionPoints = new DiagramPointCollection(new Point[] { new Point(1, 0.5) }),
                Background = Brushes.Black
            };
        }

        static DiagramShape CreateLineShape(Point position, string text, Sides anchors = Sides.Left)
        {
            return new DiagramShape()
            {
                Tag = "line",
                CanSelect = false,
                CanMove = false,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                MinHeight = 2,
                Height = 2,
                Position = position,
                Width = 50,
                BorderBrush = Brushes.Transparent,
                Background = Brushes.Black,
                Anchors = (Sides)5,
                Content = text
            };
        }

        static DiagramShape CreateLabelShape(Point position, string text, Sides anchors)
        {
            return new DiagramShape()
            {
                Tag = "label",
                Content = text,
                Position = position,
                Anchors = anchors,
                StrokeThickness = 0,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Height = 10,
                FontSize = 10,
            };
        }
    }
}




    
