using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WFActivities
{
    public abstract class Activity
    {
        public ActionBlock<ActivityActionArgs> ActionBlock { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum States { Idle, Runing, Close };

        public volatile States State = States.Idle;
        public string SessionId { get; set; }
        public int ActivityId { get; set; }

        public string ActivityName { get; set; }
        public string ActivityType { get; set; }

        public XElement Properties { get; set; }
        public Variables Variables { get; set; } 

        
        //public Dictionary<string, object> Variables { get; set; }


        public abstract Task Execute(int pinId);
        public abstract void Clear();
        public virtual int Pure(int pinId) { return 1; }
        public Activity()
        {

        }

        public Activity(string sessionId, int activityId, string activityname, string activitytype)
        {
            SessionId = sessionId;
            ActivityId = activityId;
            ActivityName = activityname;
            ActivityType = activitytype;
           // Properties = Sessions.Get(sessionId)?.Activities?.GetProperties(activityId);
           // Variables = Sessions.Get(SessionId)?.Variables;
        }


        public object GetProperty(string name)
        {
            string value = Properties?.Attribute(name).Value;
            if (value.StartsWith("@") || value.StartsWith("::"))
            {
                //Variables.LocalList.Select(i => $"{i.Key}:{(i.Value as Variable).Value}").ToList().ForEach(log.Debug);
                //log.Debug($"Variables {Variables.LocalList.Count}");
                return Variables.TryGetValue(value)?.Value ?? value;
            }
            else
            {
                return value;
            }

        }

        public object GetProperty(string expression, string name)
        {
            string str = Properties?.XPathSelectElement(expression).Attribute(name).Value;
            if (str == null) return null;
            if (str.StartsWith("@") || str.StartsWith("::"))
            {

                return Variables?.TryGetValue(str)?.Value ?? str;
            }
            else
            {
                return str;
            }

        }

        public string GetPropertyByValueNew(string name)
        {
            string value = name;
            if (value.StartsWith("@") || value.StartsWith("::"))
            {

                return Variables.TryGetValue(value)?.Value.ToString() ?? value;

            }
            else
            {
                return value;
            }

        }

        public object GetPropertyByValue(string name)
        {
            string value = name;
            if (value.StartsWith("@") || value.StartsWith("::"))
            {

                return Variables.TryGetValue(value)?.Value ?? value;

            }
            else
            {
                return value;
            }

        }


        public object GetElementProperty(string name)
        {
            XElement value = Properties?.Element("name") ?? null;
            return value;

        }

        public string GetPinName(int key, int pinId)
        {
            if (pinId <= 0)
            {
                string desc = string.Empty;
                PinsError.PinErrorDesc.TryGetValue(pinId, out desc);
                return desc;
            }
            return Properties?.Element("Pins")?.Element("Out")?.Elements("Pin")?.FirstOrDefault(item => item.Attribute("ID")?.Value == pinId.ToString())?.Attribute("Name")?.Value;

        }

        public string GetPinOutName(int key, int pinId)
        {
            if (pinId <= 0)
            {
                string desc = string.Empty;
                PinsError.PinErrorDesc.TryGetValue(pinId, out desc);
                return desc;
            }

            return Properties?.Element("Pins")?.Element("Out")?.Elements("Pin")?.FirstOrDefault(item => item.Attribute("ID")?.Value == pinId.ToString())?.Attribute("Name")?.Value;

        }
        public string GetPinInName(int key, int pinId)
        {
            try
            {
                if (pinId <= 0)
                {
                    string desc = string.Empty;
                    PinsError.PinErrorDesc.TryGetValue(pinId, out desc);
                    return desc;
                }

                return Properties?.Element("Pins")?.Element("In")?.Elements("Pin")?.FirstOrDefault(item => item.Attribute("ID")?.Value == pinId.ToString())?.Attribute("Name")?.Value;
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                log.Debug(key + ":" + pinId.ToString());
                return "ok";
            }

        }

        public void SetProperty(string name, object value)
        {
            string key = Properties?.Attribute(name).Value;
            if (!String.IsNullOrEmpty(key)) Variables.TryGetValue(key).Value = value;
        }

        public void SetProperty(string expression, string name, object value)
        {
            //log.Debug("ActivityId = " + this.ActivityId + "  " + expression + "/" + name + "->" + value.ToString());
            string key = Properties?.XPathSelectElement(expression).Attribute(name).Value;
            if (!String.IsNullOrEmpty(key)) Variables.TryGetValue(key).Value = value;
        }
    }
}
