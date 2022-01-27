
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

namespace WFBuilder.Adapters.General.Logger
{
    public class LogEscDataAdapter : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        [XtraSerializableProperty]
        [Category("LogEscData")]
        [PropertyGridEditor(TemplateKey = "VariablesEditorGeneric"), CustomDataType("Object")]
        [Display(Name = "EscData")]
        public string _EscData { get; set; } = "";

        [XtraSerializableProperty]
        [Category("LogEscData")]
        [Display(Name = "Level")]
        public LogLevel _Level { get; set; }

        public LogEscDataAdapter()
        {

            base.Name = "LogEsc";

            base.PinsIn.Add(new Pin() { id = 1, name = "in" });

            base.PinsOut.Add(new Pin() { id = 33, name = "out" });


            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
           

        }

        static LogEscDataAdapter()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(LogEscDataAdapter));
        }

       

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);


            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.Logger.LogEscDataAdapter"))["_EscData"]);
            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.General.Logger.LogEscDataAdapter"))["_Level"]);


        }

        public enum LogLevel { DEBUG, INFO, WARN, ERROR, FATAL };

       
    }
}


