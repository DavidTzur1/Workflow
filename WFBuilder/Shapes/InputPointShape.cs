using DevExpress.Diagram.Core;
using DevExpress.Xpf.Core;
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
    class InputPointShape
    {
        public static DiagramShape Create(Point position)
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
    }
}
