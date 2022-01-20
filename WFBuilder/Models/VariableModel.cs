using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [Display(Order = 0)]
        public int VariableID { get; set; }

        [XtraSerializableProperty]
        [Required(ErrorMessage = "Please enter unique name")]
        [Display(Order = 1)]
        [ValidVariableName("Name", ErrorMessage = "The variable with this name already exist")]
        public string Name { get; set; } 

        [XtraSerializableProperty]
        [Display(Order = 2)]
        [PropertyGridEditor(TemplateKey = "ValTypeEditor")]
        public ValidationDataTypeEx ValType { get; set; } = ValidationDataTypeEx.Null;

        [XtraSerializableProperty]
        [VariableValType ("ValType",ErrorMessage ="Not valid")]
        [Display(Order = 3)]
        public object Val { get; set; }

        [PropertyGridEditor(TemplateKey = "LevelScopeEditor")]
        [Display(Order = 4)]
        public LevelScopeType LevelScope { get; set; } = LevelScopeType.Local;

        [XtraSerializableProperty(XtraSerializationVisibility.SimpleCollection)]
        [ReadOnly(true)]
       // [Browsable(false)]
        public ObservableCollection<int> AdapterIDs { get; set; } = new ObservableCollection<int>();


        //[Browsable(false)]
        //public List<int> AdapterIDs = new List<int>();

        public VariableModel() 
        {
            VariableID =  MainWindow.Instance.NextVariableID++;
            Name = $"Var{VariableID.ToString()}";
        }

        public override string ToString()
        {
            return Name.ToString();
        }

    }

    public enum LevelScopeType {Local,Global}
    public enum ValidationDataTypeEx {Null,String,Integer,Double,DateTime,Currency,Boolean,Object}

}
