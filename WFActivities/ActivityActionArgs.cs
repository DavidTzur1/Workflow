using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFActivities
{
    public class ActivityActionArgs
    {
        public string SessionId { get; set; }
        public int ActivityId { get; set; }
        public int PinId { get; set; }

        public int ActionType { get; set; } = 0;

        public string Data { get; set; } = string.Empty;


        public ActivityActionArgs(string sessionId, int activityId, int pinId, int actionType = 0, string data = "")
        {
            SessionId = sessionId;
            ActivityId = activityId;
            PinId = pinId;
            ActionType = actionType;
            Data = data;

        }
    }
}
