using DevExpress.Diagram.Core;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFBuilder.Attributes;
using WFBuilder.Models;
using WFBuilder.Validations;

namespace WFBuilder.Adapters
{
    //[Range(int.MinValue,int.MaxValue) ]
    //[RegularExpression(@"^\d+.?\d{0,2}$")]

    //[PropertyGridEditor(TemplateKey = "SpinEditor")]
    //public string StrInt { get; set; }

    public class Template : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int pinsQty =-1;
        [XtraSerializableProperty]
        [Description("Property without template"), Category("Template")]
        [Display(Name = "PinsQty")]
        public int _PinsQty
        {
            get => pinsQty;

            set
            {
                pinsQty = value;
                if (pinsQty != -1)
                {
                    base.PinsInCount = value;
                    base.PinsOutCount = value;
                }
            }
        }


        [XtraSerializableProperty]
        [Description("Property without template"), Category("Template")]
        [Display(Name = "SimpleInt")]
        public int _SimpleInt { get; set; }

        [XtraSerializableProperty]
        [Description("Property Int with template"), Category("Template")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name="VarInt")]
        public string _VarInt { get; set; } = "0";

        [XtraSerializableProperty]
        [Description("Property Str with template"), Category("Template")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "VarStr")]
        public string _VarStr { get; set; } = "";

        [XtraSerializableProperty(XtraSerializationVisibility.SimpleCollection)]
        [Display(Name = "Values")]
        [Description("Property Str with template"), Category("Template")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        public ObservableCollection<int> _Values { get; set; }


        


        public Template()
        {
            base.Name = "Template";

            base.PinsIn.Add(new Pin() { id = 1, name = "In1" });
            //base.PinsIn.Add(new Pin() { id = 2, name = "In2" });
            //base.PinsIn.Add(new Pin() { id = 3, name = "In3" });
            //base.PinsIn.Add(new Pin() { id = 4, name = "In4" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nck" });

            base.PinsInCount = 1;
            base.PinsOutCount = 2;

            //Width = 100;
            Height = Math.Max(base.PinsIn.Count , base.PinsOut.Count ) * 40;
            _Values = new ObservableCollection<int>();
        }

        static Template()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Template));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);

            ////////////////////////////////////Pins Property optional///////////////////////////////////////////////////////////

            e.Properties.Add(e.CreateProxyProperty("PinsInQty", adapter => base.PinsInCount, (adapter, value) => base.PinsInCount = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins" } }));
            e.Properties.Add(e.CreateProxyProperty("PinsOutQty", adapter => base.PinsOutCount, (adapter, value) => base.PinsOutCount = value, new Attribute[] { new DisplayAttribute() { GroupName = "Pins" } }));

            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["_SimpleInt"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["_VarStr"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["_VarInt"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["_Values"]);
            // e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Template"))["PinsQty"]);


        }
    }
}
