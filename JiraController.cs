using Atlassian.Jira;
using HostJiraIntegretion.Controllers;
using HostJiraIntegretion.Models;
using Jira.SDK.Domain;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Slack.Webhooks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static HostJiraIntegretion.Controllers.Helper;
using static ConsoleApplication2.Program;

namespace HostJiraIntegretion.Controllers
{
      

    public class JiraController : Controller
    {
        public string Data,it="";

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult lo(login l)
        {
            if (ModelState.IsValid)
            {
                string username = l.username;
                string password = l.password;
                string connectionstring = "Data Source=hyd-onebox39;Initial Catalog=QueryBuilder;Persist Security Info=True;User ID=sa;Password=p@55w0rd";
                SqlConnection connection = new SqlConnection(connectionstring);
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM[QueryBuilder].[dbo].[JiraUsers] where UserName = '" + username + "'", connection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (dt.Rows.Count > 0)
                {

                     return RedirectToAction("Query");
                   

                }
                else
                {
                    ViewBag.Message = "INVALID USERNAME OR PASSWORD";
                    return RedirectToAction("Login", "Jira");
                }
            }
            return View();
        }

        public ActionResult Query()
        {
            return View();
         
        }
        public ActionResult Restcall(string txtValue)
        {
            var recipientofcritdata = ConsoleApplication2.Program.getCritical();
            return RedirectToAction("choose", "Jira");

        }
        public ActionResult choose()
        {
            string connectionstring = "Data Source=hyd-onebox39;Initial Catalog=QueryBuilder;Persist Security Info=True;User ID=sa;Password=p@55w0rd";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlDataReader dataReader;
            SqlCommand cmd = new SqlCommand("SELECT [NAME] FROM[QueryBuilder].[dbo].[JIRADETAILS]", connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            List<string> ite = new List<string>();
            while (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Data = dataReader.GetValue(i).ToString();
                    ite.Add(Data);
                }
            }
            dataReader.Close();
            List<SelectListItem> listItem = new List<SelectListItem>();   
            foreach (string User in ite)
            {
                listItem.Add(new SelectListItem() { Value = User, Text = User });
            }
            ViewBag.dropdownvalues = new MultiSelectList(listItem, "Text", "Value");
            return View();
        }
        public ActionResult Selection(string EMAIL, string MESSAGE, string SLACK)
        {
            if(MESSAGE==""&& SLACK=="")
            {
                mail(EMAIL);
            }
           else if (EMAIL == "" && MESSAGE == "")
            {
                slack();
            }
            else if (EMAIL == "" && SLACK == "")
            {
                sms();
            }

            return View();
        }

        public ActionResult mail(String EMAIL)
        {
            string[] Axioms = EMAIL.Split(new[] { "," }, StringSplitOptions.None);
            for (int a = 0; a < Axioms.Length; a++)
            { 
                Axioms[a] = Axioms[a].Trim();
                SendMail(Axioms[a], "Issue alert", "Issue is to be solved");
            }




            //string connectionstring = "Data Source=hyd-onebox39;Initial Catalog=QueryBuilder;Persist Security Info=True;User ID=sa;Password=p@55w0rd";
            //SqlConnection connection = new SqlConnection(connectionstring);
            //connection.Open();
            //SqlDataReader dataReader;
            //SqlCommand cmd = new SqlCommand("SELECT [EMAIL] FROM[QueryBuilder].[dbo].[JIRADETAILS]", connection);
            //dataReader = cmd.ExecuteReader();
            //dataReader.Read();
            //List<string> ite = new List<string>();
            //while (dataReader.Read())
            //{
            //    for (int i = 0; i < dataReader.FieldCount; i++)
            //    {
            //        Data = dataReader.GetValue(i).ToString();
            //        SendMail(Data, "Issue alert","Issue is to be solved");                  
            //        Data = null;
            //    }
            //}
            //dataReader.Close();  

           
            return null;
        }
       
            public ActionResult slack()
            {
                
                var slackClient = new SlackClient("https://hooks.slack.com/services/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");
                var slackMessage = new SlackMessage
                {
                    Channel = "#random",
                    Text = "ISSUE ALERT",
                    IconEmoji = Emoji.Ghost,
                    Username = "Ajay Varma"
                };
                slackMessage.Mrkdwn = false;
                var slackAttachment = new SlackAttachment
                {
                    Fallback = "New open task [Urgent]: <http://url_to_task|Test out Slack message attachments>",
                    Text = "New open task *[Urgent]*: <http://url_to_task|Test out Slack message attachments>",
                    Color = "#D00000",
                    Fields =
                new List<SlackField>
                    {
                    new SlackField
                        {
                            Title = "Notes",
                            Value = "This is much *easier* than I thought it would be."
                        }
                    }
                };
                slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
                slackClient.Post(slackMessage);
            return null;
            }
            public ActionResult sms()
        {
           IWebDriver driver = new ChromeDriver();
            driver.Url = ("http://site24.way2sms.com/content/index.html?");
            driver.FindElement(By.Name("username")).SendKeys("8106955177");
            driver.FindElement(By.Name("password")).SendKeys("achekuri");
            driver.FindElement(By.Id("loginBTN")).Click();
            driver.FindElement(By.Id("sendSMS")).Click();
            driver.SwitchTo().Frame("frame");
            driver.FindElement(By.Id("mobile")).SendKeys("8121137988");
            driver.FindElement(By.Id("message")).SendKeys("issue pending");
            driver.FindElement(By.Id("Send")).Click();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            driver.Close();
            return null;
        }




    }

    internal interface WebDriver
    {
        string Url { get; set; }

        object FindElement(object p);
    }
}
    
