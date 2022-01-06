using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class EntryPoints
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Dictionary<string, string> EntryPointList = new Dictionary<string, string>();

        public EntryPoints(XElement entryPoints)
        {
            string key;
            string value;
            foreach (var item in entryPoints.Elements("EntryPoint"))
            {

                key = item.Attribute("ID").Value;
                value = item.Attribute("Target").Value;
                if (!EntryPointList.ContainsKey(key))
                {
                    EntryPointList.Add(key, value);
                }
                else
                {
                    log.Error("EntryPointList not valid duplicate key  = " + key);
                }

            }
        }

        public string GetTarget(string key)
        {
            string value = string.Empty;
            EntryPointList.TryGetValue(key, out value);
            return value;
        }

        public string GetIdByIndex(int key)
        {
            return EntryPointList.ElementAtOrDefault(key).Key;

        }

        public void Clear()
        {
            EntryPointList.Clear();
        }
    }
}