using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class Broadcasts
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Dictionary<string, string> BroadcastList = new Dictionary<string, string>();



        public Broadcasts(XElement broadcasts)
        {
            string key;
            string value;
            foreach (var item in broadcasts.Elements("Broadcast"))
            {
                var list = item.Elements("Target").Select(v => v.Value).ToList();
                key = item.Attribute("ID").Value;
                value = string.Join(";", list);

                if (!BroadcastList.ContainsKey(key))
                {
                    BroadcastList.Add(key, value);
                }
                else
                {
                    log.Error("BroadcastList not valid duplicate key  = " + key);
                }

            }
        }


        public Broadcasts()
        {
        }

        public string GetTargets(string key)
        {
            string value = string.Empty;
            BroadcastList.TryGetValue(key, out value);
            return value;
        }

        public void Clear()
        {
            BroadcastList.Clear();
        }

    }
}