using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WFBuilder.Adapters
{
    
    public class Template1 : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Description("Template Adapter"), Category("Template")]
        public int PinsQty { get; set; }


        [XtraSerializableProperty]
        [Description("Template Adapter"), Category("Template")]
        [PropertyGridEditor(TemplateKey = "VariablesEditor")]
        public string VarString { get; set; }

        public Template1()
        {
            base.Name = "Template";

            base.PinsIn.Add(new Pin() { id = 1, name = "In1" });
            base.PinsIn.Add(new Pin() { id = 2, name = "In2" });


            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nck" });

            Width = 100;
            Height = Math.Max(base.PinsIn.Count + 1, base.PinsOut.Count + 1) * 30;
        }

        static Template1()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Template1));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);

            //Add prorerties of this adapter
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["PinsQty"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["VarString"]);
        }
    }
}
