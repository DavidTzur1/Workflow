using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFActivities
{
    public class Variable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly static string[] DateFormats = new string[] { "dd/MM/yyyy HH:mm:ss.fff", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH", "dd/MM/yyyy", "HH:mm:ss.fff", "HH:mm:ss", "HH:mm" };

        public string Name { get; set; }
        public string Type { get; set; }

        private object value;
        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                DateTime datetime;
                if (Type == "DateTime")
                {
                    if (DateTime.TryParseExact(value.ToString(), DateFormats, CultureInfo.CurrentCulture, DateTimeStyles.None, out datetime))
                    {
                        //this.value = value;
                        this.value = datetime;
                    }
                    else
                    {
                        log.Error("The Variable name " + Name + " Fail DateTime.TryParseExact value = " + value.ToString());
                        //this.value =  default(DateTime);
                        throw new FormatException();
                    }


                }

                int Int;
                if (Type == "Int")
                {
                    if (int.TryParse(value.ToString(), out Int))
                    {
                        //log.Debug("Name = " + this.Name + " Vale = " + value);
                        //this.value = value ;
                        this.value = Int;
                    }
                    else
                    {

                        log.Error("The Variable name " + Name + " Fail int.TryParse value = " + value.ToString());
                        //this.value = 0;
                        throw new FormatException();
                    }


                }
                double Real;
                if (Type == "Real")
                {
                    if (double.TryParse(value.ToString(), out Real))
                    {
                        //log.Debug("Name = " + this.Name + " Vale = " + value);
                        //this.value = value;
                        this.value = Real;
                    }
                    else
                    {
                        //
                        log.Error("The Variable name " + Name + " Fail double.TryParse value = " + value.ToString());
                        //this.value = 0.0d;
                        throw new FormatException();
                    }
                }
                bool Bool;
                if (Type == "Boolean")
                {
                    if (bool.TryParse(value.ToString(), out Bool))
                    {
                        //log.Debug("Name = " + this.Name + " Vale = " + value);
                        //this.value = value;
                        this.value = Bool;
                    }
                    else
                    {
                        log.Error("The Variable name " + Name + " Fail bool.TryParse value = " + value.ToString());
                        //this.value = false;
                        throw new FormatException();
                    }
                }
                if (Type == "String")
                {
                    this.value = value.ToString();
                }
                if (Type == "Object")
                {
                    this.value = value;
                }

            }
        }


        public Variable(string name, string type, object value)
        {
            Name = name;
            Type = type;
            Value = value;

        }





    }
}
