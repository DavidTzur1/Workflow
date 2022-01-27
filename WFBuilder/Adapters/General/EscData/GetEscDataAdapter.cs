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

namespace WFBuilder.Adapters.General.EscData
{
    public class GetEscDataAdapter : BaseAdapter, IXtraSupportDeserializeCollectionItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Category("GetEsc")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Object")]
        [Display(Name = "EscData")]
        public string _EscData { get; set; } = "";

       

        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Category("GetEsc")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Values")]
        public ObservableCollection<KeyValue> _Values { get; set; }

        [XtraSerializableProperty]
        [Category("GetEsc")]
        [Display(Name = "RemoveKey")]
        public bool _RemoveKey { get; set; } = false;

        [XtraSerializableProperty]
        [Category("GetEsc")]
        [Display(Name = "SkipIfNotFound")]
        public bool _SkipIfNotFound { get; set; } = false;



        public GetEscDataAdapter()
        {

            base.Name = "GetEsc";

            base.PinsIn.Add(new Pin() { id = 1, name = "in" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nak" });


            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
            _Values = new ObservableCollection<KeyValue>();

        }

        static GetEscDataAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(GetEscDataAdapter));
        }

        object IXtraSupportDeserializeCollectionItem.CreateCollectionItem(string propertyName, XtraItemEventArgs e)
        {
            if (propertyName == "_Values")
            {
                return new KeyValue();
            }
            return DiagramItemController.CreateCollectionItem(propertyName, e);
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.EscData.GetEscDataAdapter"))["_EscData"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.EscData.GetEscDataAdapter"))["_Values"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.EscData.GetEscDataAdapter"))["_RemoveKey"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.EscData.GetEscDataAdapter"))["_SkipIfNotFound"]);



        }

      
    }

   
}


