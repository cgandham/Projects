using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ws2.Controllers
{
    public class w2Controller : Controller
    {
        // GET: w2
        public ActionResult Index()
        {

            if (Request.Cookies["username"] == null && Request.Cookies["password"] == null)
            {
                //return RedirectToAction("http://localhost:58591/w1/log1");
                Uri uri = new Uri(@"http://localhost:58591/w1/log1");
                return Redirect(uri.ToString());
                //return View();
            }
           return View();
           // return RedirectToAction("http://localhost:58591/w1/log1");
        }

    }
}