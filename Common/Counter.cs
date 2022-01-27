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

    public class Counter : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int Init
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Init").ToString();
                if (int.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }

            }
        }

        public int Increment
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Increment").ToString();
                if (int.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return 1;
                }

            }
        }
        public int Decrement
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Decrement").ToString();
                if (int.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return 1;
                }

            }
        }

        public int CounterRes
        {

            set
            {
                try
                {
                    base.SetProperty("Counter", value);
                }
                catch (Exception ex)
                {
                    log.Warn(ex.Message);
                }
            }
        }
       

        int counter = 0;
        public Counter(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
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
                switch (pinId)
                {
                    case 1:
                        counter = Init;
                        CounterRes = counter;
                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                        ActionBlock.Post(activityActionArgs);
                        break;
                    case 2:
                        Interlocked.Add(ref counter, Increment);
                        CounterRes = counter;
                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                        ActionBlock.Post(activityActionArgs);

                        break;
                    case 3:
                        Interlocked.Add(ref counter, -Decrement);
                        CounterRes = counter;
                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 35);
                        ActionBlock.Post(activityActionArgs);
                        break;
                    default:
                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, PinsError.PinIdNotvalid);
                        ActionBlock.Post(activityActionArgs);
                        break;
                }


            }

            catch (Exception ex)
            {
                log.Error(ex);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, PinsError.PinException);
                ActionBlock.Post(activityActionArgs);

            }


        }
    }
}












