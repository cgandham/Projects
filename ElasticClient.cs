using System;
using Nest;
using elastic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elasticsearch.Net;

namespace elastic.elastic
{
    public class ElasticClient
    {
       
        private const string _indexName = "ab";
        public static IElasticClient _elastic { get; set; }
       

        public static void Initialize()
        {     
            _elastic = GetClient();
            
            //// Creating the index
            //_elastic.CreateIndex(_indexName, i => i
            //    .Settings(s => s
            //        .Setting("number_of_shards", 1)
            //        .Setting("number_of_replicas", 0)
            //        .Analysis(a =>
            //            a.Analyzers(x => x.Language("English", l => l.Language(Language.English))))));

            //// Creating the types
            //_elastic.Map<Models.MemberType>(x => x
            //    .Index(_indexName)
            //    .AutoMap());
        }

        private static IElasticClient GetClient()
        {
            var urlstring = new uri("http://localhost:9200");
            var settings = new connectionsettings(urlstring).disabledirectstreaming();
            //settings.DefaultIndex("Dimensions");

            //var client = new ElasticLowLevelClient(settings);
            //var descriptor = new CreateIndexDescriptor("Dimensions")
            // .Mappings(ms => ms
            // .Map<MemberModel>(m => m.AutoMap())
            // );
            //client.IndicesCreate<MemberModel>("members", descriptor, null);

            return new Nest.ElasticClient(settings);
        }

        public void Delete(int id)
        {
            _elastic.Delete<MemberType>(id, i => i.Index(_indexName));
        }

        public void Index(MemberType document)
        {
            _elastic.Index(document, i => i.Id(document.NAME.ToString())
                .Index(_indexName)
                .Type<Models.PostType>());
        }

        //public ISearchResponse<MemberType> Search(string terms)
        //{
        //    var result = _elastic.Search<MemberType>(s => s
        //        .Index(_indexName)
        //        .Type<MemberType>()
        //        .Query(q => BuildQuery(terms)));

        //    return result;
        //}

        //private QueryContainer BuildQuery(string terms)
        //{
        //    QueryContainer query = Query<MemberType>.MultiMatch(mm => mm
        //            .Query(terms)
        //            .Type(TextQueryType.MostFields)
        //            .Fields(f => f
        //                .Field(ff => ff.Title, boost: 3)
        //                .Field(ff => ff.Tags, boost: 2)
        //                .Field(ff => ff.Content, boost: 1)));

        //    return query;
        //}











    }
}