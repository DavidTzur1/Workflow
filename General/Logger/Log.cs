using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace General.Logger
{

    public class Log : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<string> Values
        {
            get
            {
                string str = String.Empty;
                return base.Properties?.Element("Values")?.Elements("Value")?.Attributes("Value")?.Select(x=>x.Value)?.ToList();
            }

        }

        public string Delimiter
        {
            get
            {
                return base.GetProperty("Delimiter") as string;
            }

        }

        public string Level
        {
            get
            {
                return base.GetProperty("Level") as string;
            }

        }
       

        public Log(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
        }
        public override void Clear()
        { }

        public override async Task Execute(int pinId)
        {


            ActivityActionArgs activityActionArgs;
            try
            {
                List<string> targets = new List<string>();
                if (Values != null)
                {
                    foreach (var item in Values)
                    {
                        targets.Add(base.GetPropertyByValue(item)?.ToString());
                    }
                }

                string Message = String.Join(Delimiter, targets.ToArray());
                // < !--Options are  "DEBUG", "INFO", "WARN", "ERROR", "FATAL". -- >
                switch (Level)
                {
                    case "DEBUG":
                        log.Debug(Message);
                        break;
                    case "INFO":
                        log.Info(Message);
                        break;
                    case "WARN":
                        log.Warn(Message);
                        break;
                    case "ERROR":
                        log.Error(Message);
                        break;
                    case "FATAL":
                        log.Fatal(Message);
                        break;
                    default:
                        log.Debug(Message);
                        break;
                }


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
