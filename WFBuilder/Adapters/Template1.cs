using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Adapters
{
    public class Template1 : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Description("Template Adapter"), Category("Template")]
        public int PinsQty { get; set; }

        public Template1()
        {
            base.Name = "Template1";

            base.PinsIn.Add(new Pin() { id = 1, name = "In" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nck" });

            Width = 100;
            Height = Math.Max(base.PinsIn.Count + 1, base.PinsOut.Count + 1) * 30;
        }

        static Template1()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Template));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);

            //Add prorerties of this adapter
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["PinsQty"]);
        }
    }
}

