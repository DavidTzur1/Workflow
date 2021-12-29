using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Validations
{

    public class ValidIntValAttribute : ValidationAttribute
    {
        private const string V = "::";
        private readonly string allowedName;
        public ValidIntValAttribute(string allowedName)
        {
            this.allowedName = allowedName;
        }
        public override bool IsValid(object value)
        {

            if (value is null) return false;
            var val = value.ToString();
            if(val.StartsWith("@"))
            {
                //val = val.TrimStart(new char[] { '@' });
                return MainWindow.Instance.VariablesInt.Where(x => x.Name == val).Count() > 0 ? true : false;
            }
            if (int.TryParse(value?.ToString(), out _))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
