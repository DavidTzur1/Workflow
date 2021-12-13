using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Display(Order = 0)]
        public int EntryPointID { get; set; }
        [XtraSerializableProperty]
        [Display(Order = 1)]
        public string EntryPointName { get; set; }
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "AdaptersEditorTemplate")] 
        [Display(Order = 2)]
        public int AdapterID { get; set; }
        [XtraSerializableProperty]
        [PropertyGridEditor(TemplateKey = "PinsEditorTemplate")]
        [Display(Order = 3)]
        public int PinID { get; set; }

        public EntryPointModel()
        {
            EntryPointID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).NextEntryPointID++;
             //AdapterID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).Adapters.FirstOrDefault().AdapterID;
            // PinID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).GetInputPins(AdapterID).FirstOrDefault().PinID;
            AdapterID = -1;
            PinID = -1;
        }
    }
}
