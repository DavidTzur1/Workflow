using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class Connections
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Dictionary<string, string> ConnectionList = new Dictionary<string, string>();

        public Connections(XElement connections)
        {
            string key;
            string value;
            foreach (var item in connections.Elements("Connection").GroupBy(s => s.Attribute("Source").Value, y => y.Attribute("Destination").Value))
            {

                key = item.Key;

                value = string.Join(";", item);
                //value = item.Attribute("Destinations").Value;
                if (!ConnectionList.ContainsKey(key))
                {
                    ConnectionList.Add(key, value);
                }
                else
                {
                    log.Error("ConnectionList not valid duplicate key = " + key);
                }

            }
        }

        public Connections()
        {
        }

        public string GetSourceConnections(string key)
        {
            if (ConnectionList.ContainsKey(key)) return ConnectionList[key];
            return String.Empty;
        }

        public void Clear()
        {
            ConnectionList.Clear();
        }
    }
}