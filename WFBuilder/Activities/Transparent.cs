using DevExpress.Diagram.Core;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Activities
{
    public class Transparent : DiagramContainerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Description("The pin qty with the control"), Category("Transparent")]

        public int PinsQty { get; set; }

        public Transparent()
        {
            base.Name = "Transparent";
            Header = "Transparent";
            PinsIn = 2;
            PinsOut = 4;
        }

        static Transparent()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Transparent));
        }
    }
}
