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
    public class BroadcastMessageModel
    {
        [XtraSerializableProperty]
        [ReadOnly(true)]
        public int BroadcastMessageID { get; set; }
        [XtraSerializableProperty]
        public string BroadcastMessageName { get; set; }
        [XtraSerializableProperty]
        public int AdapterID { get; set; }
        [XtraSerializableProperty]
        public int PinID { get; set; }

        public BroadcastMessageModel()
        {
            BroadcastMessageID = (Window.GetWindow(App.Current.MainWindow) as MainWindow).NextBroadcastMessageID++;
        }
    }
}
