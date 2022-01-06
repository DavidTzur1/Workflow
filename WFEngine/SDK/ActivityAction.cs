using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Web;
using WFActivities;

namespace WFEngine.SDK
{
    public class ActivityAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ActionBlock<ActivityActionArgs> ActionBlock;

        static ExecutionDataflowBlockOptions options;




        static ActivityAction()
        {
            options = new ExecutionDataflowBlockOptions { BoundedCapacity = 1000, MaxDegreeOfParallelism = 4 };
            ActionBlock = new ActionBlock<ActivityActionArgs>(fn, options);
        }



        //-/////////////////////////////////////////////////////ComponentDelegate//////////////////////////////////////////////////////////////

        static ConcurrentDictionary<string, int> BroadcastManager = new ConcurrentDictionary<string, int>();




        //-//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        static Action<ActivityActionArgs> fn = async item =>
        {
            try
            {
                string Pin = item.ActivityId + ":" + item.PinId;

                ///////////Get session info///////////////////////////////////////////////////////////////////
                var session = Sessions.Get(item.SessionId);
                string SessionName = session?.SessionName;
                string ServiceName = session?.Name;
                string Orig = session?.Orig;
                string Dest = session?.Dest;



                ///////////Get activity info ///////////////////////////////////////////////////////////////
                Activity activity = session?.Activities?.Get(item.ActivityId);
                string ActivityName = activity?.ActivityName;
                string PinName = activity?.GetPinOutName(item.ActivityId, item.PinId);
                var activityState = activity?.State;
                var activityType = activity?.ActivityType;

                //C///////////////////////////////////CallbackManager//////////////////////////////////////////////////////////////////////////////////////////

                CallbackManager.RaiseEvent(item.SessionId, item.ActivityId, item.PinId);


                //C///////////////////////////////////Chick if activity is terminator//////////////////////////////////////////////////////////////////////////////////////////

                if (item.ActivityId == session?.TerminateActivityId && session?.IsRootSession == true)
                {

                    //(session.Variables.TryGetValue("@EscData").Value as IDictionary<string, object>)["TerminatePinId"] = item.PinId;

                    log.Debug("Out" + "|" + Orig + "|" + Dest + "|" + ServiceName + "|" + item.SessionId + "|" + ActivityName + ":" + PinName + "|" + Pin + "|" + activityState);
                    Sessions.RemoveSession(item.SessionId);
                    return;

                }





                ///////////Log Pins Out //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (item.ActionType != ActionType.EntryPoint)

                    log.Debug("Out" + "|" + Orig + "|" + Dest + "|" + ServiceName + "|" + item.SessionId + "|" + ActivityName + ":" + PinName + "|" + Pin + "|" + activityState);


                ////////////////////////////////////////////////////////////Get next Activitys to Execute//////////////////////////////////////////////////////////

                IList<string> connections = null;

                if (item.ActionType == ActionType.Connection)
                {

                    connections = session?.Connections?.GetSourceConnections(Pin)?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    //log.Debug(String.Join(";", connections));
                }

                if (item.ActionType == ActionType.Broadcast)
                {
                    connections = session?.Broadcasts.GetTargets(item.Data)?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    //log.Debug(String.Join(";", connections));
                }

                if (item.ActionType == ActionType.EntryPoint)
                {
                    connections = session?.EntryPoints.GetTarget(item.Data)?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    //log.Debug(String.Join(";", connections));
                }

                if (connections != null)
                {
                    var tasks = connections.Select(async connection =>
                    {
                        try
                        {
                            var target = connection.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            int activityId = int.Parse(target[0]);
                            int pinId = int.Parse(target[1]);
                            Activity activityExecute = Sessions.Get(item.SessionId)?.Activities?.Get(activityId);
                            string ExecuteActivityName = activityExecute?.ActivityName;
                            string ExecutePinName = activityExecute?.GetPinInName(activityId, pinId);

                            if (activityExecute != null)
                            {
                                Pin = activityId + ":" + pinId;
                                log.Debug("In" + "|" + Orig + "|" + Dest + "|" + ServiceName + "|" + item.SessionId + "|" + ExecuteActivityName + ":" + ExecutePinName + "|" + Pin + "|" + item.ActivityId + ":" + item.PinId + "|" + activityExecute.State);


                                await activityExecute.Execute(pinId);

                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error("SessionId =" + item.SessionId, ex);
                        }
                    });

                    await Task.WhenAll(tasks);


                }

            }
            catch (Exception ex)
            {
                log.Error("SessionId = " + item.SessionId, ex);
            }
        };
    }
}