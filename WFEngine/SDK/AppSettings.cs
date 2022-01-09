using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class AppSettings
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public XElement XMLSettings { get; set; }
        static AppSettings()
        {
            XMLSettings = XElement.Load(ConfigurationManager.AppSettings["AppSettings"]);
        }

        public class DBWorkflow
        {
            public static int BatchSize
            {
                get
                {
                    int value = 1;

                    if (int.TryParse(AppSettings.XMLSettings.Element("DBWorkflow").Attribute("BatchSize").Value, out value))
                    {
                        return value;
                    }
                    return 1;
                }
            }
            public static int BatchTimer
            {
                get
                {
                    int value = 5;

                    if (int.TryParse(AppSettings.XMLSettings.Element("DBWorkflow").Attribute("BatchTimer").Value, out value))
                    {
                        return value;
                    }
                    return 5;
                }
            }
            public static int BoundedCapacity
            {
                get
                {
                    int value = 20000;

                    if (int.TryParse(AppSettings.XMLSettings.Element("DBWorkflow").Attribute("BoundedCapacity").Value, out value))
                    {
                        return value;
                    }
                    return 20000;
                }
            }
            public static int MaxDegreeOfParallelism
            {
                get
                {
                    int value = 1;

                    if (int.TryParse(AppSettings.XMLSettings.Element("DBWorkflow").Attribute("MaxDegreeOfParallelism").Value, out value))
                    {
                        return value;
                    }
                    return 1;
                }
            }
            public static string ConnectionStrings
            {
                get
                {
                    return AppSettings.XMLSettings.Element("DBWorkflow").Attribute("ConnectionStrings").Value;

                }
            }

        }

        public class ActivityType
        {
            public static string Path
            {
                get
                {
                    return AppSettings.XMLSettings.Element("ActivityType").Attribute("Path").Value;

                }
            }
        }
    }
}
