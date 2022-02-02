using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WFEngine.SDK
{
    public class WorkFlows
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, XElement> WorkflowList = new Dictionary<string, XElement>();
        
        public static XElement Get(string path)
        {
            XElement workFlow = null;

            if (!WorkflowList.ContainsKey(path))
            {
                workFlow = Workflow.Create(path);
               // workFlow = XElement.Load(path);
                WorkflowList.Add(path, workFlow);
            }
            else
            {
               workFlow = Workflow.Create(path);
               WorkflowList[path] = workFlow;

            }

            return WorkflowList[path];

        }
    }
}