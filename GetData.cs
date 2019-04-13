using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Nest;


namespace elastic.Models
{
    public class GetData
    {

        List<Dimensions> Dimensions = new List<Dimensions>();
        List<Members> Members = new List<Members>();
        DataHelper DataHelper = new DataHelper();
        public string rootpath;
        public string parentlabelpath;

        public void BuildRollup(IElasticClient client)
        {
            Dimensions = new List<Dimensions>();//DataHelper.GetDimensions();

            Dimensions.Add(new Dimensions { Tablename = "Account_123", Displayname = "100" });
            Dimensions.Add(new Dimensions { Tablename = "Company", Displayname = "102" });
            Dimensions.Add(new Dimensions { Tablename = "Scenario", Displayname = "107" });
            Dimensions.Add(new Dimensions { Tablename = "ICSegment", Displayname = "103" });
            Dimensions.Add(new Dimensions { Tablename = "Department", Displayname = "104" });
            foreach (var dim in Dimensions)
            {
                string name = dim.Tablename.ToString();
                Members = DataHelper.GetMembers(name, dim.Displayname);
                BuildTreeAndGetRoots(client, Members, name);
            }


        }

        public IEnumerable<Node> BuildTreeAndGetRoots(IElasticClient client, List<Members> actualObjects, string TypeName)
        {
            Dictionary<int, Node> lookup = new Dictionary<int, Node>();
            actualObjects.ForEach(x => lookup.Add(int.Parse(x.ID), new Node { AssociatedObject = x }));

            foreach (var item in lookup.Values)
            {

                Node proposedParent;
                if (!(TypeName.Equals("Scenario")))
                {
                    if (lookup.TryGetValue(int.Parse(item.AssociatedObject.ParentID), out proposedParent))
                    {
                        item.Parent = proposedParent;
                        // proposedParent.Children.Add(item);                    
                    }

                }


            }
            foreach (var item in lookup.Values)
            {
                rootpath = "";
                parentlabelpath = "";
                var temp = item;
                if (TypeName.Equals("Scenario"))
                {
                    //while (temp.Parent != null)
                    //{

                    rootpath = "Actual";
                    parentlabelpath = "Actual";
                    //  temp = temp.Parent;


                    //   }
                }
                else
                {
                    while (temp.Parent != null)
                    {

                        rootpath += temp.Parent.AssociatedObject.ID.ToString() + ",";
                        parentlabelpath += temp.Parent.AssociatedObject.Label.ToString() + ",";
                        temp = temp.Parent;

                    }
                }

                Members mem = new Members();
                mem.ID = item.AssociatedObject.ID;
                mem.Label = item.AssociatedObject.Label;
                mem.KeywordLabel = item.AssociatedObject.Label;
                mem.ParentID = item.AssociatedObject.ParentID;
                mem.LeftVal = item.AssociatedObject.LeftVal;
                mem.RightVal = item.AssociatedObject.RightVal;
                mem.LevelVal = item.AssociatedObject.LevelVal;
                mem.RollupOperator = item.AssociatedObject.RollupOperator;
                mem.ParentsPath = rootpath;
                mem.ParentLabelPath = parentlabelpath;
                mem.ParentDimenison = item.AssociatedObject.ParentDimenison;
                var index = ElasticSearch.ElasticConnections.Getclient().Index(mem, i => i
    .Index("index_name")
    .Type(TypeName)
    .Id(item.AssociatedObject.ID));

            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(lookup);

            return lookup.Values.Where(x => x.Parent == null);
        }



    }
}
