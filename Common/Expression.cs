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

    public class Expression : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string expression
        {
            get
            {
                return base.GetProperty("Expression") as string;
            }

        }


        public object Result
        {
            set
            {
                base.SetProperty("Result", value);
            }
        }


        int counter = 0;
        public Expression(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
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
                var expr = new NCalc.Expression(expression);
                //expr.EvaluateFunction += NCalcExtensionFunctions;
                Result = expr.Evaluate();

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













