using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WFBuilder.Models;

namespace WFBuilder
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Command]
        public void GetAdapterList()
        {
            int itemsCount = (Window.GetWindow(App.Current.MainWindow) as MainWindow).diagramControl.Items.Count();
            MessageBox.Show($"diagramControl.Items.Count()={ itemsCount}");
        }

        [Command]
        public ObservableCollection<VariableModel> Variables()
        {
            //int itemsCount = (Window.GetWindow(App.Current.MainWindow) as MainWindow).diagramControl.Items.Count();
            //MessageBox.Show($"diagramControl.Items.Count()={ itemsCount}");
            return (Window.GetWindow(App.Current.MainWindow) as MainWindow).Variables;
        }

        [Command]
        public List<int> Adapters()
        {
           // return new List<VariableModel>() { new VariableModel() { Name = "Abc", VariableID = 1, Val = 10 } };

            var list = new List<int>();
            foreach (var item in (Window.GetWindow(App.Current.MainWindow) as MainWindow).diagramControl.Items)
            {
                if (item is BaseAdapter)
                {
                    list.Add(int.Parse(item.Tag.ToString()));
                }
            }
            return list;
        }
        public MainWindowViewModel()
        {
        }
            
    }
}
