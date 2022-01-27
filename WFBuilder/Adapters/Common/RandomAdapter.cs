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
    public class RandomAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Category("Random")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Min")]
        public string _Min { get; set; } = "0";

        [XtraSerializableProperty]
        [Category("Random")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Max")]
        public string _Max { get; set; } = "0";

        [XtraSerializableProperty]
        [Category("Random")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Result")]
        public string _Result { get; set; } = "1";





        public RandomAdapter()
        {
            base.Name = "Random";

            base.PinsIn.Add(new Pin() { id = 1, name = "In" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Out" });


            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static RandomAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(RandomAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.RandomAdapter"))["_Min"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.RandomAdapter"))["_Max"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.RandomAdapter"))["_Result"]);
        }
    }
}



