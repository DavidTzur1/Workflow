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
using WFBuilder.Validations;

namespace WFBuilder.Adapters.Common
{
    public class CounterAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Category("Counter")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Init")]
        public string _Init { get; set; } = "0";

        [XtraSerializableProperty]
        [Category("Counter")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Increment")]
        public string _Increment { get; set; } = "1";

        [XtraSerializableProperty]
        [Category("Counter")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Decrement")]
        public string _Decrement { get; set; } = "1";

        [XtraSerializableProperty]
        [Category("Counter")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
       // [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Counter")]
        public string _Counter { get; set; } = "0";

        


        public CounterAdapter()
        {
            base.Name = "Count";

            base.PinsIn.Add(new Pin() { id = 1, name = "Reset" });
            base.PinsIn.Add(new Pin() { id = 2, name = "Inc" });
            base.PinsIn.Add(new Pin() { id = 3, name = "Dec" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Reout" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Incout" });
            base.PinsOut.Add(new Pin() { id = 35, name = "Decout" });

            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static CounterAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(CounterAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.CounterAdapter"))["_Init"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.CounterAdapter"))["_Increment"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.CounterAdapter"))["_Decrement"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.CounterAdapter"))["_Counter"]);


        }
    }
}

