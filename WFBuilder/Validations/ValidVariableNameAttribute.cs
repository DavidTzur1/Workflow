using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Validations
{
    public class ValidVariableNameAttribute : ValidationAttribute
    {
        private readonly string allowedName;
        public ValidVariableNameAttribute(string allowedName)
        {
            this.allowedName = allowedName;
        }
        public override bool IsValid(object value)
        {
            if (value is null) return false;
            if((MainWindow.Instance.Variables.Where(x => x.Name==value.ToString()).ToList().Count > 1))
            {
                return false;
            }
            else
            {
                return true;
            }
            

        }
    }
}
