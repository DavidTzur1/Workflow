using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WFBuilder.Validations;

namespace WFBuilder.Models
{
    public class VariableModel
    {
        [XtraSerializableProperty ]
        [Required(ErrorMessage = "Please enter unique name")]
        public string Name { get; set; }
        [XtraSerializableProperty]
        public ValidationDataTypeEx ValType { get; set; } = ValidationDataTypeEx.Integer;
        [XtraSerializableProperty]
        [VariableValType ("ValType",ErrorMessage ="Not valid")]
        public object Val { get; set; }

        public LevelScopeType LevelScope { get; set; } = LevelScopeType.Local;

        public VariableModel() { }
        
    }

    public enum LevelScopeType { Local,Global}
    public enum ValidationDataTypeEx {String,Integer,Double,Date,Currency,Boolean,Object}

}
