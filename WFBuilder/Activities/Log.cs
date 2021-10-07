using DevExpress.Diagram.Core;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Activities
{
    public class Log : DiagramContainerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [XtraSerializableProperty]
        [Description(""), Category("Log")]
        public LevelType Level { get; set; }


        [XtraSerializableProperty]
        //[PropertyGridEditor(TemplateKey = "VariableEditorTemplate")]
        [Description(""), Category("Log")]
        public string Delimiter { get; set; }

        //[PropertyGridEditor(TemplateKey = "collectionCellTemplate")]
        [XtraSerializableProperty(XtraSerializationVisibility.SimpleCollection)]
        [Description(""), Category("Log")]
        public ObservableCollection<string> Values { get; set; }

        public Log()
        {
            base.Name = "Log";
            Header = "Log";
            PinsIn = 1;
            PinsOut = 1;
            Values = new ObservableCollection<string>();
            
        }

        static Log()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Log));
        }

        public enum LevelType
        {
            Error,
            Audit,
            Caution,
            Debug,
            Warning,
            Info
        }
    }
}
