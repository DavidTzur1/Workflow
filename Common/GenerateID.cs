using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace Common
{

    public class GenerateID : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static long lastTimeStamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));



        public string Result
        {
            set
            {
                base.SetProperty("Result", value);
            }
        }


        int counter = 0;
        public GenerateID(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {

        }
        public override void Clear()
        {

        }

        public override async Task Execute(int pinId)
        {
            //ExecuteArgs executeArgs;
            ActivityActionArgs activityActionArgs;

            try
            {
                long original, newValue;
                do
                {

                    original = lastTimeStamp;
                    //long now = DateTime.UtcNow.Ticks;
                    long now = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange(ref lastTimeStamp, newValue, original) != original);
                Result = newValue.ToString();
        
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                ActionBlock.Post(activityActionArgs);

            }

            catch (Exception ex)
            {
                log.Error(ex);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                ActionBlock.Post(activityActionArgs);

            }


        }
    }
}















