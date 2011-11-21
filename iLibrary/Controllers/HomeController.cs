using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iLibrary.Infrastructure;

namespace iLibrary.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About() {
            return View();
        }

        [HttpPost]
        public ActionResult RefreshAllData() {
            ItunesCache.RefreshAll();
            return Json("Message");
        }

    }
}
