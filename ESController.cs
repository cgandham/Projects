using System.Web.Mvc;
using elastic.Models;
using Nest;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using HostAnalytics.EPM.Dashboards;
using HostAnalytics.EPM.Dashboards.Contracts.Entities;
using HostAnalytics.EPM.Dashboards.Helpers;
using HostAnalytics.EPM.QueryBuilder.Model;

namespace elastic.Controllers
{
    public class ESController : Controller
    {

        IElasticClient client;
        DataHelper DataHelper = new DataHelper();
        GetData GetData = new GetData();
        List<tooltip> SelectedData = new List<tooltip>();
        tooltip info = new tooltip();
        public ActionResult Index()
        {

            //   client = ElasticSearch.ElasticConnections.Initialize();
            //  GetData.BuildRollup(client);


            return View();
        }
        public ActionResult GetSuggestionLabels(string query)
        {

            List<tooltip> tooltip = new List<tooltip>();
            SearchRequest request = new SearchRequest("index_name")
            {
                Query = new BoolQuery()
                {
                    Must = new QueryContainer[] { new MatchPhrasePrefixQuery
                       {
                           Field = "label",
                           Query = query,
                       }
                   }
                },
                Size = 5000
            };
            var res = ElasticSearch.ElasticConnections.Getclient().Search<Members>(request);
            foreach (var hit in res.Hits)
            {
                tooltip.Add(new tooltip()
                {
                    ID = hit.Source.ID,
                    Label = hit.Source.Label,
                    ParentID = hit.Source.ParentID,
                    LeftVal = hit.Source.LeftVal,
                    RightVal = hit.Source.RightVal,
                    LevelVal = hit.Source.LevelVal,
                    RollupOperator = hit.Source.RollupOperator,
                    ParentsPath = hit.Source.ParentsPath,
                    ParentLabelPath = hit.Source.ParentLabelPath,
                    Type = hit.Type.ToString(),
                });

            }
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerialiser = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(tooltip);
            return Content(json);
        }

        public void AddMembers(string Axioms, IList<MappedDimension> Axiom)
        {
            string[] Rows = Axioms.Trim().Split(new[] { "," }, StringSplitOptions.None);
            for (int ListIndex = 0; ListIndex < Rows.Length; ListIndex++)
            {
                string CurrentAxiom = Rows[ListIndex].Trim();
                bool exists = false;
                string DimensionName = "";
                var result = ElasticSearch.ElasticConnections.Getclient().Search<Members>(s => s
                        .Index("index_name")
                        .AllTypes()
                        .Query(q => q.Term("keywordLabel", CurrentAxiom))
                        .Size(5000));

                if (result.Hits.Count > 0)
                {
                    var hit = result.Hits.ElementAt(0);
                    DimensionName = hit.Type.ToString();
                    string DimensionID = "";
                    if (hit.Type.Equals("Scenario"))
                    {
                        DimensionID = "107";
                    }
                    else
                        DimensionID = hit.Source.ParentDimenison;

                    foreach (var member in Axiom)
                    {
                        if (member.DimensionId == int.Parse(DimensionID))
                        {
                            exists = true;
                            member.Members.Add("[" + hit.Type + "].&[" + hit.Id + "]");
                            break;
                        }
                    }
                    if (!exists)
                    {
                        MappedDimension row = new MappedDimension();
                        row.DimensionId = int.Parse(DimensionID);
                        row.DimensionName = hit.Type;
                        row.LineOption = LineOptions.SELECTED;
                        row.Members = new List<string>();
                        row.Members.Add("[" + hit.Type + "].&[" + hit.Id + "]");
                        Axiom.Add(row);
                    }

                }
            }
        }

        public ActionResult SearchQuery(string query)
        {
            Dashboard _Dashboard = new Dashboard();
            _Dashboard.PageFilters = new List<MappedDimension>();
            _Dashboard.UserPreferences = new List<MappedDimension>();
            _Dashboard.Name = "Cube -  " + DateTime.Now.TimeOfDay.ToString();
            _Dashboard.LayoutType = 1;
            string[] Axioms = query.Split(new[] { "by", "BY" }, StringSplitOptions.None);
            DashboardElement _DashBoardElement = new DashboardElement();
            _DashBoardElement.Title = "Title -" + DateTime.Now.TimeOfDay.ToString();
            _DashBoardElement.ViewTitle = _DashBoardElement.Title;
            _DashBoardElement.SubTitle = "";
            _DashBoardElement.ViewSubTitle = "";
            _DashBoardElement.Type = DashboardObjectType.Chart;
            QueryDefinition queryDef = new QueryDefinition();
            MappedDimension map = new MappedDimension();
            queryDef.Page = new List<MappedDimension>();
            queryDef.Row = new List<MappedDimension>();
            queryDef.Column = new List<MappedDimension>();
            queryDef.SecondaryColumn = new List<MappedDimension>();
            if (Axioms.Length > 0)
                if (!Axioms[0].Equals(""))
                {
                    AddMembers(Axioms[0].Trim(), queryDef.Row);
                }
            if (Axioms.Length > 1)
                if (!Axioms[1].Equals(""))
                {
                    AddMembers(Axioms[1].Trim(), queryDef.Column);
                }
            _DashBoardElement.Definition = queryDef;
            _DashBoardElement.ReportingArea = 100;
            _DashBoardElement.Text = "";
            _DashBoardElement.ViewText = "";
            _DashBoardElement.Properties = new Dictionary<string, object>();
            ElasticConstants.SetDashboardElementProperties(_DashBoardElement.Properties);
            _Dashboard.DashboardElements = new List<DashboardElement>();
            _Dashboard.DashboardElements.Add(_DashBoardElement);
            _Dashboard.Properties = new Dictionary<string, object>();
            ElasticConstants.SetDashboardProperties(_Dashboard.Properties);
            var json = JsonConvert.SerializeObject(_Dashboard);
            json = ElasticConstants.RemoveGuid(json);
            PostJarvis(json);
            return Content(json);

        }



        public ActionResult search()
        {
            // client = ElasticSearch.ElasticConnections.Initialize();
            // GetData.BuildRollup(client);

            return View();
        }
        public string Jarvis()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://devsbx.haicpm.com/Auth/Login");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"Username\": \"skogara@hostanalytics.com\",\"Password\": \"Subbu123!\",\"TenantCode\": \"Summer2017Sprint3\"} ";
                streamWriter.Write(json);
                streamWriter.Close();
            }
            request.Headers.Add("Authorization", "Basic c2tvbmR1cmlAaG9zdGFuYWx5dGljcy5jb206MTI3LjAuMUBBYnM=");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
        public ActionResult PostJarvis(string postjson)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://devsbx.haicpm.com/Dashboards/Save");
            request.Method = "POST";
            request.ContentType = "application/json";

            String encoded = Jarvis();
            request.Headers.Add("APIKey", encoded);

            // request.APIKey = Jarvis();
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = postjson;
                streamWriter.Write(json);
                streamWriter.Close();
            }
            request.Headers.Add("Authorization", "Basic c2tvbmR1cmlAaG9zdGFuYWx5dGljcy5jb206MTI3LjAuMUBBYnM=");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return null;
        }




    }
}