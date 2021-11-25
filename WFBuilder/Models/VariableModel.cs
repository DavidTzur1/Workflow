using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using WFBuilder.Validations;
using static System.Net.Mime.MediaTypeNames;

namespace WFBuilder.Models
{
    [Serializable]
    public class VariableModel
    {
        [XtraSerializableProperty]
        [ReadOnly(true) ]
        [Display (Order=-9)]
        public int VariableID { get; set; }
        [XtraSerializableProperty ]
        [Required(ErrorMessage = "Please enter unique name")]
        public string Name { get; set; }
        [XtraSerializableProperty]
        public ValidationDataTypeEx ValType { get; set; } = ValidationDataTypeEx.Integer;
        [XtraSerializableProperty]
        [VariableValType ("ValType",ErrorMessage ="Not valid")]
        
        public object Val { get; set; }

        public LevelScopeType LevelScope { get; set; } = LevelScopeType.Local;

        public VariableModel() 
        {
            VariableID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).NextVariableID++;
        }
        
    }

    public enum LevelScopeType { Local,Global}
    public enum ValidationDataTypeEx {String,Integer,Double,Date,Currency,Boolean,Object}

}
