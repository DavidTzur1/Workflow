using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Behaviors
{
    public class ListBoxBehavior : Behavior<ListBoxEdit>
    {

        private void AssociatedObject_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
           // Debug.WriteLine($"AssociatedObject OldValue={e.OldValue} NewValue={e.NewValue}");
            PopupBaseEdit parent = BaseEdit.GetOwnerEdit(AssociatedObject.TemplatedParent) as PopupBaseEdit;
            parent.ClosePopup();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.EditValueChanged += AssociatedObject_EditValueChanged;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.EditValueChanged -= AssociatedObject_EditValueChanged;
            base.OnDetaching();
        }
    }
}
