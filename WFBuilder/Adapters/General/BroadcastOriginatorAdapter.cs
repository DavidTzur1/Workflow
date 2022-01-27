using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Adapters.General
{

    public class BroadcastOriginatorAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        [XtraSerializableProperty]
        [Category("Broadcast")]
        [PropertyGridEditor(TemplateKey = "BroadcastEditorTemplate")]
        [Display(Name = "Message")]
        public string _Message { get; set; }
        







        public BroadcastOriginatorAdapter()
        {
            base.Name = "BrcsOrg";

            base.PinsIn.Add(new Pin() { id = 1, name = "In" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nak" });
            base.PinsOut.Add(new Pin() { id = 35, name = "Done" });
            base.PinsOut.Add(new Pin() { id = 36, name = "Empty" });


            //Width = 100;
            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static BroadcastOriginatorAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(BroadcastOriginatorAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.BroadcastOriginatorAdapter"))["_Message"]);


        }
    }
}


