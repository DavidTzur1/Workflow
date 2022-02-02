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

namespace WFBuilder.Adapters.DataBase
{
    public class DBReaderAdapter : BaseAdapter, IXtraSupportDeserializeCollectionItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [Display(Name = "ConnectionString")]
        public string _ConnectionString { get; set; } = "";

        [XtraSerializableProperty]
        [Category("DBReader")]
        [Display(Name = "CommandType")]
        public CommandTypes _CommandType { get; set; } = CommandTypes.StoredProcedure;

        [XtraSerializableProperty]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [Display(Name = "CommandText")]
        public string _CommandText { get; set; } = "";

        [XtraSerializableProperty]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Timeout")]
        public string _Timeout { get; set; } = "30";



        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Parameters")]
        public ObservableCollection<Parameter> _Parameters { get; set; }

        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Fields")]
        public ObservableCollection<Parameter> _Fields { get; set; }

        [XtraSerializableProperty]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "ErrorCode")]
        public string _ErrorCode { get; set; } = "";

        [XtraSerializableProperty]
        [Category("DBReader")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [Display(Name = "ErrorMessage")]
        public string _ErrorMessage { get; set; } = "";



        public DBReaderAdapter()
        {

            base.Name = "DBReader";

            base.PinsIn.Add(new Pin() { id = 1, name = "Exec" });
            base.PinsIn.Add(new Pin() { id = 2, name = "Br" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nak" });
            base.PinsOut.Add(new Pin() { id = 35, name = "Done" });
            base.PinsOut.Add(new Pin() { id = 36, name = "Brn" });
            base.PinsOut.Add(new Pin() { id = 37, name = "Emp" });
            base.PinsOut.Add(new Pin() { id = 38, name = "Fail" });


            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
            _Parameters = new ObservableCollection<Parameter>();
            _Fields = new ObservableCollection<Parameter>();

        }

        static DBReaderAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(DBReaderAdapter));
        }

        object IXtraSupportDeserializeCollectionItem.CreateCollectionItem(string propertyName, XtraItemEventArgs e)
        {
            if (propertyName == "_Parameters" || propertyName == "_Fields")
            {
                return new Parameter();
            }
            return DiagramItemController.CreateCollectionItem(propertyName, e);
        }
       

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_ConnectionString"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_CommandType"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_CommandText"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_Timeout"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_Parameters"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_Fields"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_ErrorCode"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.DataBase.DBReaderAdapter"))["_ErrorMessage"]);

        }


    }

   
}





