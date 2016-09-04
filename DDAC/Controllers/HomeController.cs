using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDAC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "employee")]
        public ActionResult MaerskHome()
        {
            ViewBag.Message = "Maersk login page.";

            return View();
        }
        [Authorize(Roles = "agent")]
        public ActionResult AgentHome()
        {
            ViewBag.Message = "Agent login page.";

            return View();
        }

    }
}