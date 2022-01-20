using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WFActivities;
using static WFActivities.Activity;

namespace WFEngine.SDK
{
    public class Activities
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        volatile ConcurrentDictionary<int, Activity> ActivityList = new ConcurrentDictionary<int, Activity>();



        object locker = new object();
        XElement WorkflowXML;
        string SessionId;
        public Activities(string sessionId, XElement workflowXML)
        {
            SessionId = sessionId;
            WorkflowXML = workflowXML;

        }

        public States GetState(int key)
        {
            if (ActivityList.ContainsKey(key))
            {
                return ActivityList[key].State;
            }
            return States.Idle;
        }



        public Activity Get(int key)
        {
            try
            {

                WFActivities.Activity activity = null;
                if (ActivityList.TryGetValue(key, out activity)) return activity;


                /////////////CreateInstance/////////////////////////////////////////////////
                //log.Debug("key=" + key);

                XElement xmlActivity = WorkflowXML.Elements("Activity").First(item => item.Attribute("ID").Value == key.ToString());

                // assembly name
                //string assemblyName = "WorkflowEngine";
                //string assemblyName = "WFActivities";

                //namespace.class name

                string name = xmlActivity.Attribute("Name").Value;
                string adapterType = xmlActivity.Attribute("Type").Value;
                //log.Debug($"Name={name} Type={type}");
                //string Namespace = xmlActivity.Attribute("Namespace").Value;
                string assemblyName = ActivityType.Data[adapterType].AssemblyName; 
                string Namespace = ActivityType.Data[adapterType].Namespace;
                string type = ActivityType.Data[adapterType].Type;
                string fullClassName = Namespace + "." + type;

                string objectToInstantiate = fullClassName + "," + assemblyName;
                //log.Debug("objectToInstantiate=" + objectToInstantiate);
                var objectType = Type.GetType(objectToInstantiate);

                //object[] args = new object[] { m_callFlowEngine, xmlComponent };
                object[] args = new object[] { SessionId, key, name, type };
                //log.Debug($"2Name={name} Type={type}");
                activity = Activator.CreateInstance(objectType, args) as WFActivities.Activity;
                //log.Debug($"3Name={name} Type={type}");
                activity.Properties = Sessions.Get(SessionId)?.Activities?.GetProperties(key);
                activity.ActionBlock = ActivityAction.ActionBlock;
                //activity.Variables.LocalList = Sessions.Get(SessionId)?.Variables.LocalList;
                //WFActivities.Variables.GlobalList = Variables.GlobalList;
                activity.Variables = Sessions.Get(SessionId)?.Variables;


                if (ActivityList.TryAdd(key, activity))
                {
                    return activity;
                }
                else
                {
                    ActivityList.TryGetValue(key, out activity);
                    return activity;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }



        public XElement GetProperties(int key)
        {
            return WorkflowXML.Elements("Activity").First(item => item.Attribute("ID").Value == key.ToString()).Element("Properties");

        }





        public void Clear()
        {
            foreach (var item in ActivityList)
            {
                item.Value.Clear();
            }
            //ActivityList.Clear();
        }
    }
}