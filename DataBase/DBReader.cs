using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace DataBase
{

    public class DBReader : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ConnectionString
        {
            get
            {
                return base.GetProperty("ConnectionString").ToString();
            }
        }

        public CommandType CommandType
        {
            get
            {
                switch (base.GetProperty("CommandType").ToString())
                {
                    case "StoredProcedure":
                        return CommandType.StoredProcedure;
                    case "Text":
                        return CommandType.Text;
                    default:
                        return CommandType.StoredProcedure; ;
                }

            }
        }

        public string CommandText
        {
            get
            {
                return base.GetProperty("CommandText").ToString();
            }
        }

        public int Timeout
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Timeout").ToString();
                if (int.TryParse(str, out result))
                {
                    return result;
                }
                else
                {
                    return 30;
                }

            }
        }



        public List<SqlParameter> Parameters
        {
            get
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                IEnumerable<XElement> parameters = base.Properties?.Element("Parameters")?.Elements("Value");
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        string name = "@" + base.GetPropertyByValue(item.Attribute("Name").Value).ToString();
                        string value = base.GetPropertyByValue(item.Attribute("Value").Value).ToString();
                        sqlParameters.Add(new SqlParameter(name, value));
                    }
                }
                return sqlParameters;
            }

        }

        public IEnumerable<XElement> Fields1
        {
            get
            {
                string str = String.Empty;
                return base.Properties.Element("Fields").Element("Values").Elements("Value");
            }

        }
        public List<KeyValuePair<string,string>> Fields
        {
            get
            {
                List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
                
                var fiels = base.Properties?.Element("Fields")?.Elements("Value");
                if (fiels != null)
                {
                    foreach(var item in fiels)
                    {
                        
                        string name =  base.GetPropertyByValue(item.Attribute("Name").Value).ToString();
                        string value =item.Attribute("Value").Value;
                        //log.Debug($"get filds name ={name} value={value}");
                        result.Add(new KeyValuePair<string, string>(name,value));
                    }

                }
                return result;
            }

        }

        public int ErrorCode
        {
            set
            {
                base.SetProperty("ErrorCode", value);
            }
        }

        public string ErrorMessage
        {
            set
            {
                base.SetProperty("ErrorMessage", value);
            }
        }


        public DBReader(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {

            semaphoreSlim = new SemaphoreSlim(1, 1);
            cts = new CancellationTokenSource();
        }
        public override void Clear()
        {
            // log.Debug("Clear");
            cts.Cancel();
            cts.Dispose();
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

                            using (var connection = new SqlConnection(ConnectionString))
                            {
                                using (var command = new SqlCommand(CommandText, connection))
                                {
                                    command.CommandType = CommandType;
                                    command.Parameters.AddRange(Parameters.ToArray());
                                    command.CommandTimeout = Timeout;
                                    activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                                    ActionBlock.Post(activityActionArgs);

                                    try
                                    {

                                        State = States.Runing;
                                        semaphoreSlim.Release();
                                        await connection.OpenAsync(cts.Token);
                                        var reader = await command.ExecuteReaderAsync(cts.Token);
                                        
                                        while (await reader.ReadAsync(cts.Token))
                                        {
                                            foreach (var item in Fields)
                                            {
                                                Variables.TryGetValue(item.Value).Value = reader[item.Key].ToString();
                                            }
                                            break;
                                            //log.Debug(reader["CustomerName"].ToString());
                                        }


                                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, reader.HasRows?35:37);
                                        ActionBlock.Post(activityActionArgs);
                                        State = States.Idle;
                                    }

                                    catch (SqlException e)
                                    {
                                        log.Error($"error code = {e.Number} Error message={e.Message}");
                                        ErrorCode = e.Number;
                                        ErrorMessage = e.Message;
                                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, cts.IsCancellationRequested ? 36 : 38);
                                        ActionBlock.Post(activityActionArgs);
                                        State = States.Idle;
                                    }

                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        ErrorMessage = ex.Message;
                                        activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 38);
                                        ActionBlock.Post(activityActionArgs);
                                        State = States.Idle;
                                    }

                                }
                            }
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
                            cts.Cancel();
                            State = States.Idle;
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



            catch (Exception ex)
            {
                log.Error(ex);
                State = States.Idle;

                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                ActionBlock.Post(activityActionArgs);

                semaphoreSlim.Release();

            }
        }
    }
}














