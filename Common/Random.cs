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

    public class Random : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int Min
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Min").ToString();
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

        public int Max
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Max").ToString();
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


        public int Result
        {
            set
            {
                base.SetProperty("Result", value);
            }
        }


        int counter = 0;
        public Random(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
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
                Result = new System.Random().Next(Min,Max);
                
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














