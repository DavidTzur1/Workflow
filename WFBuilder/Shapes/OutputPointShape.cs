using DevExpress.Diagram.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WFBuilder.Shapes
{
    public class OutputPointShape
    {
        public static DiagramShape Create(Point position)
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
                Background = Brushes.Green
            };
        }
    }
}
