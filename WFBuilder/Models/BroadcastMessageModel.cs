using DevExpress.Utils.Serializing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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

        [XtraSerializableProperty(XtraSerializationVisibility.SimpleCollection)]
        public ObservableCollection<BrodcastTargetModel> BrodcastTarges { get; set; }
        
       

        public BroadcastMessageModel()
        {
            BroadcastMessageID =MainWindow.Instance.NextBroadcastMessageID++;
            BrodcastTarges = new ObservableCollection<BrodcastTargetModel>();
            BrodcastTarges.CollectionChanged += BrodcastTarges_CollectionChanged;
        }

        public void BrodcastTarges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // MessageBox.Show("BrodcastTarges_CollectionChanged");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (BrodcastTargetModel item in e.OldItems)
                {
                        MainWindow.Instance.UpdateBackgroundInputPointShape(item.AdapterID, item.PinID, Brushes.Black);
                }
            }
        }
    }
}
