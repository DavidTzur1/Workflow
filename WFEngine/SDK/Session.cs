using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class Session
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string SessionType { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;

        public bool SessionState { get; set; } = true;

        public int TerminateActivityId { get; set; } = 1;

        public string Orig { get; set; }
        public string Dest { get; set; }
        public string Name { get; set; }
        public string WorkFlowPath { get; set; }





        public Activities Activities { get; set; }
        public Variables Variables { get; set; }
        public Connections Connections { get; set; }
        public Broadcasts Broadcasts { get; set; }
        public EntryPoints EntryPoints { get; set; }

        public bool IsRootSession { get; set; } = false;
       // public cVaxCallSession VaxCallSession { get; set; } = null;



        //Dictionary<string, object> Resource = new Dictionary<string, object>();

        public Action<string, string, string> Callback = null;





        public Session(string sessionId, string orig, string dest, string serviceName, string workflowFile, int terminateActivityId = 1, IDictionary<string, object> escData = null)
        {

            SessionId = sessionId;
            Orig = orig;
            Dest = dest;
            Name = serviceName;
            WorkFlowPath = workflowFile;
            TerminateActivityId = terminateActivityId;



            XElement workflowXML = WorkFlows.Get(workflowFile);

          

            Variables = new Variables(workflowXML.Element("Variables"), escData);


            Activities = new Activities(sessionId, workflowXML.Element("Activities"));

            Connections = new Connections(workflowXML.Element("Connections"));

            Broadcasts = new Broadcasts(workflowXML.Element("Broadcasts"));

            EntryPoints = new EntryPoints(workflowXML.Element("EntryPoints"));



        }


        public Session()
        {

        }





        public void Clear()
        {
            Activities.Clear();
            //Variables.Clear();
            //Connections.Clear();
            //Broadcasts.Clear();
            //EntryPoints.Clear();
            CallbackManager.Clear(SessionId);
            //if (VaxCallSession != null)
            //{
            //    VaxCallSession.CloseCallSession();
            //}


        }

    }
}