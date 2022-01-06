using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Web;
using WFEngine.Models;

namespace WFEngine.SDK
{
    public class DBWorkflowLog
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // The size of a single batch to add to the database.
        static int insertBatchSize = 1;
        static int batchTimer = 5;
        static int boundedCapacity = 20000;
        static int maxDegreeOfParallelism = 1;
        static System.Timers.Timer timeOutTimer;


        static BatchBlock<DBLogModel> batchRequests;
        static ActionBlock<DBLogModel[]> insertRequests;

        static ExecutionDataflowBlockOptions options;



        static DBWorkflowLog()
        {

            insertBatchSize = AppSettings.DBWorkflow.BatchSize;
            batchTimer = AppSettings.DBWorkflow.BatchTimer;
            boundedCapacity = AppSettings.DBWorkflow.BoundedCapacity;
            maxDegreeOfParallelism = AppSettings.DBWorkflow.MaxDegreeOfParallelism;

            batchRequests = new BatchBlock<DBLogModel>(insertBatchSize);

            var timeOut = TimeSpan.FromSeconds(batchTimer);
            timeOutTimer = new System.Timers.Timer(timeOut.TotalMilliseconds);
            timeOutTimer.Elapsed += (s, e) => batchRequests.TriggerBatch();

            options = new ExecutionDataflowBlockOptions { BoundedCapacity = boundedCapacity, MaxDegreeOfParallelism = maxDegreeOfParallelism };

            insertRequests = new ActionBlock<DBLogModel[]>(a => InsertRequests(a), options);

            batchRequests.LinkTo(insertRequests);

            // batchRequests.Complete();

            timeOutTimer.Start();

        }

        public static void Post(DBLogModel req)
        {
            batchRequests.Post(req);
        }

        static async void InsertRequests(DBLogModel[] requests)
        {
            timeOutTimer.Stop();
            string connectionString = ConfigurationManager.ConnectionStrings["WorkflowConn"].ConnectionString;
            try
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(connectionString))

                {

                    copy.DestinationTableName = "WorkflowLog";
                    DataTable dataTable = new DataTable("WorkflowLog");
                    dataTable.Columns.Add("MachineName", typeof(string));
                    dataTable.Columns.Add("Channel", typeof(string));
                    dataTable.Columns.Add("SessionId", typeof(string));
                    dataTable.Columns.Add("StartDate", typeof(DateTime));
                    dataTable.Columns.Add("EndDate", typeof(DateTime));
                    dataTable.Columns.Add("ServiceName", typeof(string));
                    dataTable.Columns.Add("PAI", typeof(string));
                    dataTable.Columns.Add("Orig", typeof(string));
                    dataTable.Columns.Add("Dest", typeof(string));
                    dataTable.Columns.Add("ExtraInfo", typeof(string));
                    dataTable.Columns.Add("ExceptionTrace", typeof(string));
                    dataTable.Columns.Add("StatusCode", typeof(int));
                    dataTable.Columns.Add("TerminatePinId", typeof(string));


                    //-///////////////////////////////////////////////////////////////////
                    foreach (var req in requests)
                    {

                        dataTable.Rows.Add(req.MachineName, req.Channel, req.SessionId, req.StartDate, req.EndDate, req.ServiceName, req.PAI, req.Orig, req.Dest, req.ExtraInfo, req.ExceptionTrace, req.StatusCode, req.TerminatePinId);

                    }

                    copy.ColumnMappings.Add("MachineName", "MachineName");
                    copy.ColumnMappings.Add("Channel", "Channel");
                    copy.ColumnMappings.Add("SessionId", "SessionId");
                    copy.ColumnMappings.Add("StartDate", "StartDate");
                    copy.ColumnMappings.Add("EndDate", "EndDate");
                    copy.ColumnMappings.Add("ServiceName", "ServiceName");
                    copy.ColumnMappings.Add("PAI", "PAI");
                    copy.ColumnMappings.Add("Orig", "Orig");
                    copy.ColumnMappings.Add("Dest", "Dest");
                    copy.ColumnMappings.Add("ExtraInfo", "ExtraInfo");
                    copy.ColumnMappings.Add("ExceptionTrace", "ExceptionTrace");
                    copy.ColumnMappings.Add("StatusCode", "StatusCode");
                    copy.ColumnMappings.Add("TerminatePinId", "TerminatePinId");


                    await copy.WriteToServerAsync(dataTable);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            finally
            {

                timeOutTimer.Start();

            }
        }
    }
}