using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Attributes
{
    public class CustomDataTypeAttribute : Attribute
    {
        public string Parameter { get; }
        public CustomDataTypeAttribute(string parameter)
        {
            Parameter = parameter;
        }
    }
}
