using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elastic.elastic
{
    public class Elasticserach
    {
        ElasticClient client = null;
       
        public void Elasticsearch()
        {
            var uri = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(uri);
            client = new ElasticClient(settings);
            settings.DefaultIndex("city");
           
        }
    }
}