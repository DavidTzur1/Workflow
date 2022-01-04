using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WFEngine.SDK
{
    public class StartUp
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> StartUpList;
        private static HttpClient Client = new HttpClient();
        static StartUp()
        {
            log.Debug("StartUp");
            try
            {
                StartUpList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationManager.AppSettings["StartUp"]));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public StartUp()
        {

        }

        public static void Start()
        {
            foreach (var item in StartUpList)
            {
                try
                {
                    if (!item.StartsWith("_"))
                        Client.GetAsync(item);
                    else
                        log.Info($"The url:{item} is in comment ");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

            }


        }
    }
}
