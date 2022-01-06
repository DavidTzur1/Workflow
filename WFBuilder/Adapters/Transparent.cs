using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Adapters
{

    public class Transparent : BaseAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int pinsQty = 1;
        [XtraSerializableProperty]
        [Description("Transparent"), Category("Transparent")]
        [Display(Name = "PinsQty")]
        public int _PinsQty
        {
            get => pinsQty;

            set
            {
                pinsQty = value;
                base.PinsInCount = value;
                base.PinsOutCount = value;
                //if (pinsQty != -1)
                //{
                //    base.PinsInCount = value;
                //    base.PinsOutCount = value;
                //}
            }
        }


       




        public Transparent()
        {
            base.Name = "Transparent";

            base.PinsIn.Add(new Pin() { id = 1, name = "In1" });
    
            base.PinsOut.Add(new Pin() { id = 33, name = "Out1" });


            //Width = 100;
            Height = Math.Max(base.PinsIn.Count, base.PinsOut.Count) * 40;
          
        }

        static Transparent()
        {
            DiagramControl.ItemTypeRegistrator.Register(typeof(Transparent));
        }

        public override void AddProperties(DiagramCustomGetEditableItemPropertiesEventArgs e)
        {
            //Add Base properties
            base.AddProperties(e);

            
            ///////////////////////////////////Add prorerties of this adapter//////////////////////////////////////

            e.Properties.Add(TypeDescriptor.GetProperties(Type.GetType("WFBuilder.Adapters.Transparent"))["_PinsQty"]);


        }
    }
}
