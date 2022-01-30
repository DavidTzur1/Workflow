using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WFEngine.Controllers
{
    public class TestHttpController : ApiController
    {
        //http://10.11.32.43:82/api/TestHttpController?id=789
        [HttpGet]
        [Route("api/TestHttpController")]
        public async Task<string> Get(string id)
        {
            await Task.Delay(10000);
            return "OK";
        }
    }
}
