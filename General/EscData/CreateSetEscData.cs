
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace General.EscData
{

    public class CreateSetEscData : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IDictionary<string, object> DataExchange
        {
            get
            {
                return base.GetProperty("EscData") as IDictionary<string, object>;

            }
            set
            {
                base.SetProperty("EscData", value);
            }
        }

        public IEnumerable<XElement> Values
        {
            get
            {
                string str = String.Empty;
                return base.Properties.Element("Values").Elements("Value");
            }

        }
       
       

        public CreateSetEscData(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
        }
        public override void Clear()
        {
            DataExchange.Clear();
        }

        public override async Task Execute(int pinId)
        {


            ActivityActionArgs activityActionArgs;
            try
            {
                DataExchange = new Dictionary<string, object>();


                foreach (var item in Values)
                {
                    string key = base.GetPropertyByValue(item.Attribute("Key").Value) as string;
                    object value = base.GetPropertyByValue(item.Attribute("Value").Value);

                    if (DataExchange.ContainsKey(key))
                    {
                        DataExchange[key] = value;

                    }
                    else
                    {

                        DataExchange.Add(key, value);
                    }

                }

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

 





