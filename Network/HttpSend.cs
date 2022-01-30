using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFActivities;

namespace Network
{

    public class HttpSend : Activity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static ServiceProvider serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        static IHttpClientFactory httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        public string Method
        {
            get
            {
                return base.GetProperty("Method").ToString();
            }
        }
        public string URL
        {
            get
            {
                return base.GetProperty("URL").ToString();
            }
        }
       
        public int Timeout
        {
            get
            {
                int result = 0;
                string str = base.GetProperty("Timeout") as string;
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

        public IEnumerable<XElement> Headers
        {
            get
            {
                string str = String.Empty;
                return base.Properties?.Element("Headers")?.Elements("Value");
            }

        }

        public string Result
        {
            set
            {
                base.SetProperty("Result", value);
            }
        }



        SemaphoreSlim semaphoreSlim;
        CancellationTokenSource source;

        public HttpSend(string sessionId, int activityId, string name, string type) : base(sessionId, activityId, name, type)
        {
            
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }
        public override void Clear()
        {
           // log.Debug("Clear");
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
                            source = new CancellationTokenSource();
                            var client = httpClientFactory.CreateClient();
                            client.Timeout = TimeSpan.FromSeconds(Timeout);
                            HttpRequestMessage req = new HttpRequestMessage(Method == "GET"?HttpMethod.Get:HttpMethod.Post, URL);
                            if (Headers != null)
                            {
                                foreach (var item in Headers)
                                {
                                    string name = base.GetPropertyByValue(item.Attribute("Name").Value).ToString();
                                    string value = base.GetPropertyByValue(item.Attribute("Value").Value).ToString();
                                    req.Headers.TryAddWithoutValidation(name, value);
                                }
                            }

                                //log.Debug($"URL={URL}");
                                var task = client.SendAsync(req, source.Token);
                           
                            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 33);
                            ActionBlock.Post(activityActionArgs);
                            State = States.Runing;
                            semaphoreSlim.Release();
                            var response = await task;
                            response.EnsureSuccessStatusCode();
                            Result = await response.Content.ReadAsStringAsync();

                            activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 35);
                            ActionBlock.Post(activityActionArgs);
                            State = States.Idle;
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
            
            catch (HttpRequestException ex)
            {
                log.Error(ex.InnerException.Message);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 37);
                ActionBlock.Post(activityActionArgs);
                State = States.Idle;

            }
            catch (TaskCanceledException ex) when (source.IsCancellationRequested)
            {
                log.Error(ex.Message);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 36);
                ActionBlock.Post(activityActionArgs);
                State = States.Idle;
            }
           

            catch (TaskCanceledException ex)
            {
                // Handle cancellation.
                log.Debug("Request timed out");
                //log.Error(ex.Message);
                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 37);
                ActionBlock.Post(activityActionArgs);
                State = States.Idle;
            }
           
            catch (Exception ex)
            {
                //Nak=36
                log.Error(ex);
                State = States.Idle;

                activityActionArgs = new ActivityActionArgs(SessionId, ActivityId, 34);
                ActionBlock.Post(activityActionArgs);

                semaphoreSlim.Release();

            }
        }
    }
}












