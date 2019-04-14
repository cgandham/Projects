using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp2.Controllers
{
    public class Home2Controller : Controller
    {
        [Authorize]
        // GET: Home2
        public ActionResult Index()
        {
            return View();
        }
    }
}