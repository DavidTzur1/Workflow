using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFActivities;

namespace CommonActivities
{
    public class Assign : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object Source
        {
            get
            {
                return base.GetProperty("Source");
            }
        }

        public object Destination
        {
            get
            {
                return base.GetProperty("Destination");
            }
            set
            {
                base.SetProperty("Destination", value);
            }
        }
        public override void Clear()
        {
           // throw new NotImplementedException();
        }

        public Assign(string sessionId, int actvityId, string name, string type) : base(sessionId, actvityId, name, type)
        {
            //semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public override async Task Execute(int pinId)
        {
            ActivityActionArgs activityActionArgs;

            try
            {
               // log.Debug($"Pre Destination = {Destination.ToString()}");
                Destination = Source;
               // log.Debug($"Destination = {Destination.ToString()}");
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                ActionBlock.Post(activityActionArgs);

            }

            catch (Exception ex)
            {
                log.Error(ex);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                ActionBlock.Post(activityActionArgs);

            }
        }
    }
}
