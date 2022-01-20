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
    public class AssignAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Description("Property Str with template"), Category("Assign")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Source")]
        public string _Source { get; set; } = "";

        [XtraSerializableProperty]
        [Description("Property Str with template"), Category("Assign")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Destination")]
        public string _Destination { get; set; } = "";



        public AssignAdapter()
        {
            base.Name = "Assign";

            base.PinsIn.Add(new Pin() { id = 1, name = "in" });

            base.PinsOut.Add(new Pin() { id = 33, name = "ok" });
            base.PinsOut.Add(new Pin() { id = 34, name = "err" });

            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static AssignAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(AssignAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.Assign"))["_Source"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.Assign"))["_Destination"]);


        }
    }
}
