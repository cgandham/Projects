using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using HostAnalytics.EPM.SSO.Saml20;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace w2.Controllers
{
    [Authorize]
    public class w11Controller : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult log(string username, string password)
        {
            if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
            {

                return View("index");
            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult log(w2.Models.log obj)
        {
            if (FormsAuthentication.Authenticate(obj.username, obj.password))
            {
                FormsAuthentication.SetAuthCookie(obj.username, false);
                FormsAuthentication.RedirectFromLoginPage(obj.username, true);
                Session["islogged"] = true;
            }

            if(!string.IsNullOrEmpty(obj.SAMLRequest) && !string.IsNullOrEmpty(obj.RelayState))
            {
                return RedirectToActionPermanent("SSO", new Models.SSOModel() { SAMLRequest = obj.SAMLRequest, RelayState = obj.RelayState });
            }
            return View();
        }

        public ActionResult home(w2.Models.log obj)
        {
            return View("home");
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult SSO(Models.SSOModel obj)
        {
            if (Session["islogged"] != null && (bool)Session["islogged"] == true)
            {
                ArtifactResponseType response = new ArtifactResponseType();
                DateTime dt = DateTime.Parse("01/06/2017");
                string s1 = dt.ToString("dd-MM-yyyy");
                DateTime dtnew = DateTime.Parse(s1);
                response.ID = s1;
                response.Issuer = new NameIDType();
                response.Issuer.Value = "http://ws11.com";
                response.Destination = "http://ws22.com";
                var serializer = new XmlSerializer(typeof(ArtifactResponseType));
                var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
                // Models.sso ssoObj = new Models.sso();
                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, response);
                    }

                    obj.SAMLResponse = stringWriter.ToString();
                    var base64EncodedBytes = Encoding.UTF8.GetBytes(obj.SAMLResponse);
                    var base64EncodedXml = Convert.ToBase64String(base64EncodedBytes);

                    obj.SAMLResponse = base64EncodedXml;
                    obj.RelayState = Guid.NewGuid().ToString();

                    return View("SSO", obj);
                }
            }
            else /*if (Session["islogged"] == null && (bool)Session["islogged"] == false)*/
            {    //redirect to login page, 
                 //return redirecttoaction("login", obj);
                 //after login
                 //return redirecttoaction("SSO", obj);
                Session["islogged"] = true;
                var log = new Models.log();
                log.SAMLRequest = obj.SAMLRequest;
                log.RelayState = obj.RelayState;
                return View("log", new Models.log() { SAMLRequest = obj.SAMLRequest, RelayState = obj.RelayState });
            }
        }
   
        
        
    }
}