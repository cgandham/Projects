using System.Web.Mvc;
using System.Web;
using System;



namespace Ws1.Controllers
{
    public class w1Controller : Controller
    {
        // GET: w1
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult log1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult log1(Ws1.Models.log1 obj)
        {
            if (obj.username != null && obj.password!=null)
            {
                // add info to cookie
                HttpCookie mycookie = new HttpCookie("username");
                mycookie.Value = "ss";
                mycookie.Expires = DateTime.Now.AddHours(10);
                Response.Cookies.Add(mycookie);

                HttpCookie mycookie1 = new HttpCookie("password");
                mycookie1.Value = "ss";
                mycookie1.Expires = DateTime.Now.AddHours(10);
                Response.Cookies.Add(mycookie1);


              return  RedirectToAction("Home");
            }
                return View();      
        }

        public ActionResult Home(Ws1.Models.log1 obj)
        {
            return View();
        }

    }
}