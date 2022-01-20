using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using WFActivities;
using WFEngine.Models;
using WFEngine.SDK;

namespace WFEngine.Controllers
{
    public class TestController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // http://10.11.32.43:8081/api/TestController?path=D:\Workflow\Test\Assign.xml
        //http://10.11.32.43:82/api/TestController?Dest=0544165500&Orig=Web&Service=Template&Params=""
        //http://10.11.32.43:82/api/TestController?id=0544165500
        //http://10.11.32.43:82/api/TestController?Dest=0544165500&Orig=Web&Service=Log&Params=""

        //public string Get(string id)
        //{
        //    return "xxxxxxxxxxxxxxx";
        //}

        [HttpGet]
        [Route("api/TestController")]
        public string Get([FromUri] string Dest, string Orig, string Service, string Params)
        {
            DateTime startdate = DateTime.Now;
            DateTime endDate;
            string channel = "Web";
            string sessionId = "";

            try
            {
                // log.Debug(path);
                XElement serviceXML = Services.GetService("Name", Service);

                if (serviceXML == null)
                {
                    string error = "The Service " + Service + " Not Found in the Services.xml.";
                    log.Error(error);
                    DBWorkflowLog.Post(new DBLogModel { Channel = channel, SessionId = sessionId, StartDate = startdate, EndDate = DateTime.Now, ServiceName = Service, Orig = Orig, Dest = Dest, ExtraInfo = error, StatusCode = 2005 });

                    return error;
                }
                string path = serviceXML.Attribute("Path").Value;
                XElement workFlowXML = WorkFlows.Get(path);

                Dictionary<string, Object> escData = new Dictionary<string, object>();
                escData.Add("Orig", Orig);
                escData.Add("Dest", Dest);
                escData.Add("Service", Service);
                escData.Add("Params", Params);
                sessionId = Sessions.CreateSession(Orig, Dest, Service, path, 1, escData);
                Sessions.Get(sessionId).IsRootSession = true;
                log.Debug("WorkflowSessions.CreateSession = " + sessionId + " Workflow Path = " + path);

                ActivityActionArgs activityActionArgs = new ActivityActionArgs(sessionId, 0, 0, ActionType.EntryPoint, "0");
                ActivityAction.ActionBlock.Post(activityActionArgs);

                endDate = DateTime.Now;
                DBWorkflowLog.Post(new DBLogModel { Channel = channel, SessionId = sessionId, StartDate = startdate, EndDate = endDate, ServiceName = Service, Orig = Orig, Dest = Dest, ExtraInfo = "", StatusCode = 0 });
                return path;
            }
            catch (Exception ex)
            {
                endDate = DateTime.Now;
               // DBWorkflowLog.Post(new DBLogModel { Channel = channel, SessionId = sessionId, StartDate = startdate, EndDate = endDate, ServiceName = Service, Orig = Orig, Dest = Dest, ExtraInfo = "", ExceptionTrace = ex.ToString(), StatusCode = 4 });
                log.Error(ex);
                return "Error Create CallFlowSession";
            }
        }
    }
}


    

