using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IHS.ZlotePrzeboje.Models;

namespace IHS.ZlotePrzeboje.Controllers
{
    public class HomeController : Controller
    {
        public static IList<Proposition> _propositions = new List<Proposition>();
        public ActionResult Index()
        {
            return View(_propositions.OrderBy(x=>x));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPOST(Proposition model)
        {
            model.Id = _propositions.Any() ? _propositions.Max(x => x.Id) + 1 : 0;

            _propositions.Add(model);
            return RedirectToAction("Index");
        }

        public ActionResult VoteUP(int id)
        {
            var proposition = _propositions.Single(x => x.Id == id);
            proposition.VotesUP++;

            return RedirectToAction("Index");
        }

        public ActionResult Player()
        {
            return View();
        }
    }
}
