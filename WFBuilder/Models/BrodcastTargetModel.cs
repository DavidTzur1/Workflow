using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Models
{
    [Serializable]
    public class BrodcastTargetModel
    {
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "AdaptersEditorTemplate")]
        [Display(Order = 1)]
        public int AdapterID { get; set; } = -1;
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "PinsEditorTemplate")]
        [Display(Order = 2)]
        public int PinID { get; set; } = -1;
    }
}
