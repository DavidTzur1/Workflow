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
    public class LabelShape
    {

        public LabelShape()
        {

        }
       public static  DiagramShape Create(Point position, string text, Sides anchors)
        {
            return new DiagramShape()
            {
                Content = text,
                Position = position,
                Anchors = anchors,
                StrokeThickness = 0,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                CanAttachConnectorBeginPoint = false,
                CanAttachConnectorEndPoint = false,
                Height = 30
            };
        }
    }
}
