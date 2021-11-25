using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WFBuilder.Models
{
    [Serializable]
    public class EntryPointModel
    {
        [XtraSerializableProperty]
        [ReadOnly(true)]
        public int EntryPointID { get; set; }
        [XtraSerializableProperty]
        public string EntryPointName { get; set; }
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "Adapters")]
        public int AdapterID { get; set; }
        [XtraSerializableProperty]
        public int PinID { get; set; }

        public EntryPointModel()
        {
            EntryPointID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).NextEntryPointID++;
        }
    }
}
