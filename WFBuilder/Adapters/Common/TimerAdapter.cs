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
using WFBuilder.Attributes;

namespace WFBuilder.Adapters.Common
{
    public class TimerAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Category("Timer")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [Display(Name = "TimeOut")]
        public int _TimeOut { get; set; } = 10;

        [XtraSerializableProperty]
        [Category("Timer")]
        [Display(Name = "SingleUsage")]
        public bool _SingleUsage { get; set; } = false;



        public TimerAdapter()
        {
            base.Name = "Timer";

            base.PinsIn.Add(new Pin() { id = 1, name = "in" });
            base.PinsIn.Add(new Pin() { id = 2, name = "Br" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "To" });
            base.PinsOut.Add(new Pin() { id = 35, name = "Brn" });
            base.PinsOut.Add(new Pin() { id = 36, name = "Nak" });
            base.PinsOut.Add(new Pin() { id = 37, name = "Abrt" });

            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static TimerAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(TimerAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.TimerAdapter"))["_TimeOut"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.TimerAdapter"))["_SingleUsage"]);


        }
    }
}

