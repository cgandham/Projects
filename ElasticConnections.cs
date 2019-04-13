using elastic.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elastic.ElasticSearch
{
    public class ElasticConnections
    {
        
        DataHelper DataHelper = new DataHelper();
               
        public static IElasticClient Initialize()
        {
            var urlString = new Uri("http://localhost:9200"); // ES default url
            var settings = new ConnectionSettings(urlString).DisableDirectStreaming().DefaultIndex("index_name");
             ElasticClient client = new ElasticClient(settings);         
            if (client.IndexExists("index_name").Exists)
            {
                 var response = client.DeleteIndex("index_name");            
            }
            var createIndexResponse = client.CreateIndex("index_name", c => c.Settings(s => s.NumberOfShards(1).NumberOfReplicas(0)).Mappings(m => m.Map<Members>(d => d.AutoMap())));

            return client;
        }
        public static IElasticClient Getclient()
        {
            var urlString = new Uri("http://localhost:9200"); // ES default url
            var settings = new ConnectionSettings(urlString).DisableDirectStreaming().DefaultIndex("index_name");
            ElasticClient client = new ElasticClient(settings);
            return client;

        }



    }
}