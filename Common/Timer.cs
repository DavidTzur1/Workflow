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

    public class Timer : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private volatile bool Isfirstloop = true;
        public double TimeOut
        {
            get
            {
                double result = 0;
                string str = base.GetProperty("TimeOut") as string;
                if (double.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return 10;
                }

            }
        }

        public bool SingleUsage
        {
            get
            {
                bool result = false;
                string str = base.GetProperty("SingleUsage") as string;
                if (bool.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return false;
                }

            }
        }

        SemaphoreSlim semaphoreSlim;
        CancellationTokenSource source;

        public Timer(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
            source = new CancellationTokenSource();
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }
        public override void Clear()
        {
            //log.Debug("Clear");
            source.Cancel();
            source.Dispose();
        }

        public override async Task Execute(int pinId)
        {
            ActivityActionArgs activityActionArgs;
            await semaphoreSlim.WaitAsync();
            try
            {
                switch (pinId)
                {
                    case 1:

                        if (States.Idle == State)
                        {
                            Isfirstloop = true;
                            do
                            {

                                Task task = Task.Delay(TimeSpan.FromSeconds(this.TimeOut), source.Token);

                                if (Isfirstloop)
                                {

                                    activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                                    ActionBlock.Post(activityActionArgs);
                                    Isfirstloop = false;
                                    State = States.Runing;
                                    semaphoreSlim.Release();
                                }

                                await task;
                                if (SingleUsage) State = States.Idle;

                                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                                ActionBlock.Post(activityActionArgs);


                            } while (!SingleUsage);




                        }
                        else
                        {
                            //Pin not active

                            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, PinsError.PinNotActive);
                            ActionBlock.Post(activityActionArgs);
                            semaphoreSlim.Release();
                        }
                        break;

                    case 2:
                        
                        if (States.Runing == State)
                        {
                            source.Cancel();
                            State = States.Idle;
                            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 35);
                            ActionBlock.Post(activityActionArgs);
                            semaphoreSlim.Release();
                        }

                        else
                        {
                            //Pin not active
                            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, PinsError.PinNotActive);
                            ActionBlock.Post(activityActionArgs);
                            semaphoreSlim.Release();

                        }
                        break;
                }




            }
            catch (TaskCanceledException)
            {



            }
            catch (Exception ex)
            {
                //Nak=36
                log.Error(ex);
                State = States.Idle;

                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 36);
                ActionBlock.Post(activityActionArgs);

                semaphoreSlim.Release();

            }
        }
    }
}











