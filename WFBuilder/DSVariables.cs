using DevExpress.Diagram.Core;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WFBuilder.Models;

namespace WFBuilder
{
    public class DSVariables :DiagramImage, IXtraSupportDeserializeCollectionItem
    {
        [XtraSerializableProperty(XtraSerializationVisibility.Collection, useCreateItem: true)]
        public ObservableCollection<VariableModel> Variables { get; set; }
        public DSVariables()
        {
            Position = new Point(30, 30);
            Width = 50;
            Height = 50;
            StretchMode = DevExpress.Diagram.Core.StretchMode.Stretch;
            Background = Brushes.Gray;
            Image = new BitmapImage(new Uri("../../Images/Apply.png", UriKind.Relative));
            Variables = new ObservableCollection<VariableModel>();


        }

        object IXtraSupportDeserializeCollectionItem.CreateCollectionItem(string propertyName, XtraItemEventArgs e)
        {
            if (propertyName == "Variables")
            {
                return new VariableModel();
            }
            return DiagramItemController.CreateCollectionItem(propertyName, e);
        }

        static DSVariables()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(DSVariables));
        }

        public void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            e.Properties.Add(TypeDescriptor.GetProperties(typeof(DSVariables))["Variables"]);
        }
               
    }
}
