using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFActivities;

namespace General
{
    public class Transparent : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int PinsQty
        {
            get
            {
                return (int)base.GetProperty("PinsQty");
            }
            set
            {
                base.SetProperty("PinsQty", value);
            }
        }

        public Transparent(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
        }
        public override void Clear()
        { }

        public override async Task Execute(int pinId)
        {


            ActivityActionArgs activityActionArgs;
            int pinIdOut = pinId + 32;

            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, pinIdOut);
            ActionBlock.Post(activityActionArgs);


        }


    }
}

