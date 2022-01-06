using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFEngine.Models
{

    public class DBLogModel
    {
        static string machineName;

        public string MachineName = machineName;
        public string Channel { get; set; } = String.Empty;
        public string SessionId { get; set; } = String.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ServiceName { get; set; } = String.Empty;
        public string PAI { get; set; } = String.Empty;
        public string Orig { get; set; } = String.Empty;

        public string Dest { get; set; } = String.Empty;
        public string ExtraInfo { get; set; } = String.Empty;
        public string ExceptionTrace { get; set; } = String.Empty;
        public int StatusCode { get; set; }
        public string TerminatePinId { get; set; } = String.Empty;
        static DBLogModel()
        {
            machineName = Environment.MachineName;
        }
    }
}