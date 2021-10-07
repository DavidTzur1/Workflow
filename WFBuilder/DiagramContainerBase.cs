using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Diagram.Core;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;

namespace WFBuilder
{
    public class DiagramContainerBase : DiagramContainer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [XtraSerializableProperty]
        public int ID { get; set; }
        public int PinsIn { get; set; } = 1;
        public int PinsOut { get; set; } = 1;


        public DiagramContainerBase()
        {
            log.Debug("Create DiagramContainerBase");
            ItemsCanSelect = false;
            ItemsCanMove = false;
            DragMode = ContainerDragMode.ByAnyPoint;
            ShowHeader = true;
            CanAttachConnectorBeginPoint = false;
            CanAttachConnectorEndPoint = false;
            
            Width = 120;
            Height = 230;


        }

        static DiagramContainerBase()
        {
           DiagramControl.ItemTypeRegistrator.Register(typeof(DiagramContainerBase));
        }
    }
}
