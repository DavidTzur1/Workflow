using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WFEngine.Models;

namespace WFEngine.SDK
{


    public class ActivityType
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, ActivityTypeModel> Data = new Dictionary<string, ActivityTypeModel>();

        static ActivityType()
        {
            try
            {
                XElement xml = XElement.Load(AppSettings.ActivityType.Path);
                foreach(var item in xml.Elements("Activity") )
                {
                    ActivityTypeModel model = new ActivityTypeModel() {Type=item.Attribute("Type").Value, AssemblyName = item.Attribute("AssemblyName").Value,Namespace= item.Attribute("Namespace").Value };
                    Data.Add(item.Attribute("ItemKind").Value, model);
                }

               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
    }
}