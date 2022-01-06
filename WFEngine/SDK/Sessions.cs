using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WFEngine.SDK
{
    public class Sessions
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static long lastTimeStamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));

        public static ConcurrentDictionary<string, Session> SessionList = new ConcurrentDictionary<string, Session>();

        public static string CreateSession(string orig, string dest, string serviceName, string workflowFilepath, int TerminateActivityId = 1, IDictionary<string, object> escData = null)
        {

            string SessionId = UniqueID;

            //Session Session = new Session(orig, workflowXML, SessionID, service, TerminateActivityId);

            Session Session = new Session(SessionId, orig, dest, serviceName, workflowFilepath, TerminateActivityId, escData);
            SessionList.TryAdd(SessionId, Session);
            return SessionId;
        }

        public static string CreateChildSession(string parentSessionId, string workflowFilepath, string childServiceName, int TerminateActivityId = 1, IDictionary<string, object> escData = null)
        {
            var obj = Sessions.Get(parentSessionId);

            string SessionId = parentSessionId + "." + UniqueID;
            string serviceName = obj.Name + "." + childServiceName;
            string orig = obj.Orig;
            string dest = obj.Dest;

            Session Session = new Session(SessionId, orig, dest, serviceName, workflowFilepath, TerminateActivityId, escData);
            SessionList.TryAdd(SessionId, Session);
            return SessionId;


        }





        public static bool RemoveSession(string sessionId)
        {


            Session session = null;



            if (SessionList.TryRemove(sessionId, out session))
            {
                session.Clear();
                log.Debug("RemoveSession :" + sessionId);
                return true;
            }



            return false;

        }

        public static Session Get(string sessionId)
        {
            Session session = null;
            SessionList.TryGetValue(sessionId, out session);

            return session;
        }

        public static string UniqueID
        {
            get
            {
                long original, newValue;
                do
                {
                    original = lastTimeStamp;
                    //long now = DateTime.UtcNow.Ticks;
                    long now = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange(ref lastTimeStamp, newValue, original) != original);
                string uniqueID = newValue.ToString(); //+ ConfigurationManager.AppSettings[Environment.MachineName];
                return uniqueID;
            }
        }

        public static string GetInfo()
        {
            ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>();
            string info = "";
            try
            {
                foreach (var item in SessionList.Values)
                {
                    cd.AddOrUpdate(item.WorkFlowPath, 1, (key, oldValue) => oldValue + 1);
                }

                foreach (var item in cd)
                {
                    info += item.Key + "=" + item.Value + Environment.NewLine;
                }
                return info;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ex.ToString();
            }
        }
    }
}