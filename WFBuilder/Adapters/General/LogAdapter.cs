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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFBuilder.Attributes;
using WFBuilder.Models;
using WFBuilder.Validations;

namespace WFBuilder.Adapters.General
{
    public class LogAdapter : BaseAdapter  , IXtraSupportDeserializeCollectionItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Category("Log")]
        [Display(Name = "Values")]
        public ObservableCollection<StringValue> _Values { get; set; }

        [XtraSerializableProperty]
        [Category("Log")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Delimiter")]
        public string _Delimiter { get; set; } = "";

        [XtraSerializableProperty]
        [Category("Log")]
        [Display(Name = "Level")]
        public LogLevel _Level { get; set; }

        public LogAdapter()
        {
            
            base.Name = "Log";

            base.PinsIn.Add(new Pin() { id = 1, name = "in" });

            base.PinsOut.Add(new Pin() { id = 33, name = "out" });
            

            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
            _Values = new ObservableCollection<StringValue>();

        }

        static LogAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(LogAdapter));
        }

        object IXtraSupportDeserializeCollectionItem.CreateCollectionItem(string propertyName, XtraItemEventArgs e)
        {
            if (propertyName == "_Values")
            {
                return new StringValue();
            }
            return DiagramItemController.CreateCollectionItem(propertyName, e);
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.LogAdapter"))["_Values"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.LogAdapter"))["_Delimiter"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.LogAdapter"))["_Level"]);


        }

        public enum LogLevel {DEBUG, INFO, WARN, ERROR, FATAL };

        [Serializable]
        public class StringValue
        {
            [XtraSerializableProperty]
            [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String|Integer|DateTime")]
            [ValidVal("String", ErrorMessage = "The value is invalid")]
            [Display(Name = "Value")]
            public string Value { get; set; } = "";

        }
    }
}
