using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MO.Controllers
{
    public class OrdControlController : ApiController
    {
        // GET api/ordcontrol
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ordcontrol/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/ordcontrol
        public void Post([FromBody]string value)
        {
        }

        // PUT api/ordcontrol/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ordcontrol/5
        public void Delete(int id)
        {
        }
    }
}
