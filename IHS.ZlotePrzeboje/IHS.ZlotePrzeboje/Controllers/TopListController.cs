using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IHS.ZlotePrzeboje.Controllers
{
    public class TopListController : ApiController
    {
        public string Get()
        {
            var propositions = HomeController._propositions;
            var proposition = propositions.OrderBy(x => x).FirstOrDefault();
            if (proposition != null)
            {
                propositions.Remove(proposition);
                return proposition.URL;
            }
            else
            {
                return "";
            }
        }
    }
}
