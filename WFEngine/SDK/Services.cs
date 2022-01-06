using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class Services
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static public XElement XMLService { get; set; }

        static Services()
        {
            XMLService = XElement.Load(ConfigurationManager.AppSettings["XMLServices"]);
        }

        public static XElement GetService(string key, string value)
        {
            try
            {
                if (key == "Dest")
                {
                    value = value.StartsWith("+972") ? "0" + value.Substring(4) : value;

                    string firstTen = value?.Substring(0, value.Length >= 10 ? 10 : value.Length);
                    return XMLService.Element("Services").Elements("Service").FirstOrDefault(item => item.Attribute(key).Value == firstTen);

                }
                return XMLService.Element("Services").Elements("Service").FirstOrDefault(item => item.Attribute(key).Value == value);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public static bool Refresh()
        {
            try
            {
                XMLService = XElement.Load(ConfigurationManager.AppSettings["XMLServices"]);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return false;
            }
        }


    }
}