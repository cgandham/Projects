
namespace ws22.Controllers
{
    using HostAnalytics.EPM.SSO.Saml20;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Xml;
    using System.Xml.Serialization;

    [Authorize]
    public class w22Controller : Controller
    {
 
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult log11()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult log11(ws22.Models.log11 obj)
        {
            if (FormsAuthentication.Authenticate(obj.username, obj.password))
            {
                FormsAuthentication.SetAuthCookie(obj.username, true);
                FormsAuthentication.RedirectFromLoginPage(obj.username, true);
                //Session["islogged"] = true;

                return RedirectToAction("hi");
            }

            //if (Membership.ValidateUser(obj.username, obj.password))
            //{
            //    FormsAuthentication.SetAuthCookie(obj.username, true);
            //    FormsAuthentication.RedirectFromLoginPage(obj.username, true);

            //    return RedirectToAction("hi");
            //}

            return View();
        }

        public ActionResult hi(ws22.Models.log11 obj)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult hey()
        {
            AuthnRequestType request = new AuthnRequestType();
            request.IssueInstant = DateTime.Now;
            request.Issuer = new NameIDType();
            request.Issuer.Value = "http://ws22.com";
            var serializer = new XmlSerializer(typeof(AuthnRequestType));
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            Models.sso ssoObj = new Models.sso();
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, request);
                }

                ssoObj.SAMLRequest = stringWriter.ToString();
                var base64EncodedBytes = Encoding.UTF8.GetBytes(ssoObj.SAMLRequest);
                var base64EncodedXml = Convert.ToBase64String(base64EncodedBytes);

                ssoObj.SAMLRequest = base64EncodedXml;
                ssoObj.RelayState = Guid.NewGuid().ToString();
            }
            return View("sso", ssoObj);
        }

       






}
}
