using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        db_a094d4_demoEntities db = new db_a094d4_demoEntities();
        public ActionResult Index()
        {
            var date_from = DateTime.Today.AddDays(-1);
                var date_to =  DateTime.Today.AddDays(1);
            
            return View();
        }

        public JsonResult production_data ()
        {
            var discovery_data = (from d in db.discovery where d.time >= DateTime.Today.AddDays(-1) && d.time <= DateTime.Today.AddDays(1) select d).Count();
            var rediscovery_data = (from d in db.rediscovery where d.time >= DateTime.Today.AddDays(-1) && d.time <= DateTime.Today.AddDays(1) select d).Count();
            return Json(discovery_data,JsonRequestBehavior.AllowGet);

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
    }
}