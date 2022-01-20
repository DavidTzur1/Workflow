using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WFActivities
{
    public class Variables
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, Variable> LocalList = new Dictionary<string, Variable>();

        public static Dictionary<string, Variable> GlobalList = new Dictionary<string, Variable>();


        public Variables(XElement Variables, IDictionary<string, object> escData = null)
        {
            foreach (var item in Variables.Elements("Variable"))
            {
                string name = item.Attribute("Name").Value;
                string type = item.Attribute("Type").Value;
                object value = item.Attribute("Value").Value;
               

                if (name == "@EscData")
                {
                    value = escData;
                }

                var val = new Variable(name, type, value);

                if (name.StartsWith("@"))
                {
                    if (!LocalList.ContainsKey(name))
                    {
                        LocalList.Add(name, val);
                    }
                    else
                    {
                        log.Error("VariablesList not valid duplicate key = " + name);
                    }
                }

                if (name.StartsWith("::"))
                {
                    if (!GlobalList.ContainsKey(name))
                    {
                        GlobalList.Add(name, val);
                    }
                    else
                    {
                        //log.Error("VariablesList not valid duplicate key = " + name);
                    }
                }

            }

        }



        public Variable TryGetValue(string key)
        {
            Variable variable = null;
            if (key.StartsWith("@"))
            {
                LocalList.TryGetValue(key, out variable);
               // log.Debug($"variable Name={variable.Name} Type={variable.Type} Value={variable.Value}");

            }
            else
            {
                GlobalList.TryGetValue(key, out variable);
            }
            return variable;
        }

        public bool TrySetValue(string key, object value)
        {
            if (key.StartsWith("@"))
            {
                if (LocalList.ContainsKey(key))
                {
                    LocalList[key].Value = value;
                    return true;
                }
            }
            else
            {
                if (GlobalList.ContainsKey(key))
                {
                    GlobalList[key].Value = value;
                    return true;
                }
            }
            return false;

        }

        public void Clear()
        {
            LocalList.Clear();
        }
    }
}
