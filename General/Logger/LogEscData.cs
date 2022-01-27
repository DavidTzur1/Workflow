

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace General.Logger
{

    public class LogEscData : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IDictionary<string, object> EscData
        {
            get
            {
                return base.GetProperty("EscData") as IDictionary<string, object>;

            }
        }

        

        public string Level
        {
            get
            {
                return base.GetProperty("Level") as string;
            }

        }


        public LogEscData(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
        }
        public override void Clear()
        { }

        public override async Task Execute(int pinId)
        {


            ActivityActionArgs activityActionArgs;


            try
            {
                switch (Level)
                {
                    case "DEBUG":
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Debug);
                        break;
                    case "INFO":
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Info); ;
                        break;
                    case "WARN":
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Warn);

                        break;
                    case "ERROR":
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Error);
                        break;
                    case "FATAL":
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Fatal);
                        break;
                    default:
                        EscData.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(log.Debug);
                        break;
                }


                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                ActionBlock.Post(activityActionArgs);

            }
            catch (Exception)
            {
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                ActionBlock.Post(activityActionArgs);


            }

        }


    }
}

