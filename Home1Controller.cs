using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp1.Controllers
{
    public class Home1Controller : Controller
    {
        [Authorize]
        // GET: Home1
        public ActionResult Index()
        {
            return View();
        }
    }
}