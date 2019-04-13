using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace EmailHTMLtable
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Program p = new Program();
            p.SendMail();
        }
           void  SendMail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEMAIL"]);
            mail.To.Add(ConfigurationManager.AppSettings["ToEMAIL"]);
            string[] cc = ConfigurationManager.AppSettings["CC"].Split(',');
            foreach(string c in cc)
            {
                c.Replace("\n", " ");
                c.Replace("\r", " ");
                mail.CC.Add(c);
            }
            
            //    mail.CC.Add(ConfigurationManager.AppSettings["CC2"]);
            mail.Subject = "NCover Coverage Report"; 
            mail.IsBodyHtml = true;
            mail.Body = HtmlBody();
           
            var thread = new Thread(() => MailThread(mail));
            thread.Start();

        }

       string HtmlBody()
           {
            string dest = "file://hyd-qatools/NCover/";
            string today = DateTime.Today.ToString("dd-MM-yyyy");
            string report = dest + today + ".html";
            FileWebRequest myRequest = (FileWebRequest)WebRequest.Create(report);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;

        }
          void MailThread(MailMessage mail)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FromEMAIL"], ConfigurationManager.AppSettings["FromEMAILPassword"]);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

    }  
}



