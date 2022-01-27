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
    public class ExpressionAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Category("Expression")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [Display(Name = "Expression")]
        public string _Expression { get; set; } = "0";

        [XtraSerializableProperty]
        [Category("Expression")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer|String|Double")]
        //[ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Result")]
        public string _Result { get; set; } = "1";

        



        public ExpressionAdapter()
        {
            base.Name = "Expression";

            base.PinsIn.Add(new Pin() { id = 1, name = "In" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nak" });

            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;

        }

        static ExpressionAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(ExpressionAdapter));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.ExpressionAdapter"))["_Expression"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Common.ExpressionAdapter"))["_Result"]);
        }
    }
}


