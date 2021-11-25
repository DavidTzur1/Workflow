using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
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
        public List<VariableModel> GetVarables()
        {
            return new List<VariableModel>() { new VariableModel() { Name = "Abc", VariableID = 1, Val = 10 } };
        }
        public MainWindowViewModel()
        {
        }
            
    }
}
