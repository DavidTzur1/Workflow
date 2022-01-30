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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WFBuilder.Attributes;
using WFBuilder.Models;
using WFBuilder.Validations;

namespace WFBuilder.Adapters.Network
{
    public class HttpSendAdapter : BaseAdapter, IXtraSupportDeserializeCollectionItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [XtraSerializableProperty]
        [Category("Http")]
        [Display(Name = "Method")]
        public HTTPMethod _Method { get; set; } = HTTPMethod.GET;

        [XtraSerializableProperty]
        [Category("Http")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "URL")]
        public string _URL { get; set; } = "";



        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        [Category("Http")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [Display(Name = "Headers")]
        public ObservableCollection<Header> _Headers { get; set; }

        [XtraSerializableProperty]
        [Category("Http")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Content")]
        public string _Content { get; set; } = "";

        [XtraSerializableProperty]
        [Category("Http")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Integer")]
        [ValidVal("Integer", ErrorMessage = "The value is invalid")]
        [Display(Name = "Timeout")]
        public string _Timeout { get; set; } = "";

        [XtraSerializableProperty]
        [Category("Http")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Result")]
        public string _Result { get; set; } = "";





        public HttpSendAdapter()
        {

            base.Name = "HTTPSend";

            base.PinsIn.Add(new Pin() { id = 1, name = "send" });
            base.PinsIn.Add(new Pin() { id = 2, name = "bk" });

            base.PinsOut.Add(new Pin() { id = 33, name = "Ack" });
            base.PinsOut.Add(new Pin() { id = 34, name = "Nak" });
            base.PinsOut.Add(new Pin() { id = 35, name = "done" });
            base.PinsOut.Add(new Pin() { id = 36, name = "bkn" });
            base.PinsOut.Add(new Pin() { id = 37, name = "failed" });


            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
            _Headers = new ObservableCollection<Header>();

        }

        static HttpSendAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(HttpSendAdapter));
        }

        object IXtraSupportDeserializeCollectionItem.CreateCollectionItem(string propertyName, XtraItemEventArgs e)
        {
            if (propertyName == "_Headers")
            {
                return new Header();
            }
            return DiagramItemController.CreateCollectionItem(propertyName, e);
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_Method"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_URL"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_Headers"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_Content"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_Timeout"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Network.HttpSendAdapter"))["_Result"]);
            

        }

        


    }
    public class Header
    {

        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Value")]
        public string Value { get; set; }
    }

    public enum HTTPMethod { GET,POST }


}



