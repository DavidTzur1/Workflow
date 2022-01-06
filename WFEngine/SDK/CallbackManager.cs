using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFEngine.SDK
{
    public class CallbackManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static ConcurrentDictionary<string, MyDelegate> RegisterList = new ConcurrentDictionary<string, MyDelegate>();
        public delegate void MyDelegate(int a, int b);



        static public bool Register(string targetSessionId, int targetActivityId, MyDelegate myDelegate)
        {
            try
            {
                string key = targetSessionId + "_" + targetActivityId;
                //string value = senderSessionId + "." + senderActivityId;
                RegisterList.AddOrUpdate(key, myDelegate, (k, v) => v + myDelegate);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }

        }

        static public bool UnRegister(string targetSessionId, int targetActivityId, MyDelegate myDelegate)
        {

            try
            {
                string key = targetSessionId + "_" + targetActivityId;
                RegisterList.AddOrUpdate(key, myDelegate, (k, v) => v - myDelegate);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }

        }

        static public void RaiseEvent(string sessionId, int activityId, int pinId)
        {
            string key = sessionId + "_" + activityId;
            MyDelegate myEvent;
            if (RegisterList.TryGetValue(key, out myEvent))
            {
                if (myEvent != null)
                    myEvent(activityId, pinId);
            }

        }



        static public void Clear(string sessionId)
        {
            MyDelegate temp = null;
            var keysToRemove = RegisterList.Keys.Where(key => key.StartsWith(sessionId + "_")).ToList();
            keysToRemove.ForEach(log.Debug);
            keysToRemove.ForEach(key => RegisterList.TryRemove(key, out temp));

        }
    }
}