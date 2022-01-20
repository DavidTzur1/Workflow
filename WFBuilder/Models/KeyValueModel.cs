using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFBuilder.Attributes;
using WFBuilder.Validations;

namespace WFBuilder.Models
{
   
    [Serializable]
    public class KeyValueModel
    {
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("String")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Key")]
        public string Key { get; set; } = "";
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("All")]
        [ValidVal("String", ErrorMessage = "The value is invalid")]
        [Display(Name = "Value")]
        public string Value { get; set; } = "";

    }
}
