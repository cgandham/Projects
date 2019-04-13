
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nest;

namespace elastic.Models
{
    public class DataHelper
    {
        public static IElasticClient _elastic { get; set; }
        public static string connectionstring = System.Configuration.ConfigurationManager.
    ConnectionStrings["DBConnection"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlCommand DBCommand;
        SqlDataReader DataReader;
        public List<Members> GetMembers(string name, string dimID)
        {
            string TableName = GetTableName(name);
            List<Members> segmentMembers = new List<Members>();
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                if (name.Equals("Scenario"))
                {
                    DBCommand = new SqlCommand("SELECT [IDX] ,[CODE],[PARENT_IDX],LEFT_VAL,RIGHT_VAL,LEVEL_VAL,ROLLUP_OPERATOR   FROM " + TableName, connection);

                }
                else
                {
                    DBCommand = new SqlCommand("SELECT [LINE_ID] ,[LINE_NAME],[PARENT_LINE_ID],LEFT_VAL,RIGHT_VAL,LEVEL_VAL,ROLLUP_OPERATOR FROM " + TableName, connection);

                }
                DataReader = DBCommand.ExecuteReader();
                DataTable dt = new DataTable();
                while (DataReader.Read())
                {
                    var segmentMember = new Members()
                    {
                        ID = DataReader.GetValue(0).ToString(),
                        Label = DataReader.GetValue(1).ToString(),
                        ParentID = DataReader.GetValue(2).ToString(),
                        LeftVal = DataReader.GetValue(3).ToString(),
                        RightVal = DataReader.GetValue(4).ToString(),
                        LevelVal = DataReader.GetValue(5).ToString(),
                        RollupOperator = DataReader.GetValue(6).ToString(),

                        ParentDimenison = dimID

                    };
                    segmentMembers.Add(segmentMember);
                }
                DataReader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return segmentMembers;
        }


        //public string GetLineID(string DimensionName)
        //{
        //    string LineID = "",TableName = "",type = "";
        //    var res = ElasticSearch.ElasticConnections.Getclient().Search<Members>(s => s
        //             .Index("index_name")
        //             .AllTypes()
        //             .Query(q => q.Term("keywordLabel", DimensionName))
        //             .Size(5000));
        //    foreach (var hit in res.Hits)
        //    {                
        //        type = hit.Type.ToString();
        //        TableName = GetTableName(type);
        //    }
        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed) connection.Open();
        //        DBCommand = new SqlCommand("SELECT  [LINE_ID]  FROM " + TableName + " WHERE [LINE_NAME] ='" + DimensionName + "' AND ROLLUP_ID = 1  ", connection);
        //        DataReader = DBCommand.ExecuteReader();
        //        DataTable dt = new DataTable();
        //        while (DataReader.Read())
        //        {
        //            LineID = DataReader.GetValue(0).ToString();
        //        }
        //        DataReader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }


        //    return LineID;
        //}

        //public string GetDimensionID(string type)
        //{
        //    string DimensionID = "";          
        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed) connection.Open();
        //        DBCommand = new SqlCommand("SELECT  DIMENSION_ID  FROM S_SEGMENT where DISPLAY_NAME ='" + type + "' AND ACTIVE = 1   ", connection);
        //        DataReader = DBCommand.ExecuteReader();
        //        DataTable dt = new DataTable();
        //        while (DataReader.Read())
        //        {
        //            DimensionID = DataReader.GetValue(0).ToString();
        //        }
        //        DataReader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return DimensionID;
        //}

        public List<Dimensions> GetDimensions()
        {


            List<Dimensions> Dims = new List<Dimensions>();
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                DBCommand = new SqlCommand("select SRC_TABLE , DISPLAY_NAME from S_SEGMENT WHERE ACTIVE = 1  ", connection);
                DataReader = DBCommand.ExecuteReader();
                DataTable dt = new DataTable();
                while (DataReader.Read())
                {
                    var Dim = new Dimensions()
                    {
                        Tablename = DataReader.GetValue(0).ToString(),
                        Displayname = DataReader.GetValue(1).ToString(),

                    };
                    Dims.Add(Dim);

                }
                DataReader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return Dims;
        }

        public string GetTableName(string type)
        {
            string TableName = "";
            //if (type == "Account")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG1]";
            //if (type == "Company")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG2]";
            //if (type == "ICSegment")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG3]";
            //if (type == "Department")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG4]";
            //if (type == "CostCenter")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG5]";
            //if (type == "BusinessCenter")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG6]";
            //if (type == "ProductLine")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG7]";
            //if (type == "Geography")
            //    TableName = "[BVTCOPY].[dbo].[OLAP_SEG8]";
            //if (type == "Scenario")
            //    TableName = "[DIM_SCENARIO]";
            if (type == "Account_123")
            {
                TableName = "[OLAP_SEG1]";
            }
            else if (type == "Scenario")
            {
                TableName = "[DIM_SCENARIO]";
            }
            if (type == "Company")
                TableName = "[OLAP_SEG2]";
            if (type == "ICSegment")
                TableName = "[OLAP_SEG3]";
            if (type == "Department")
                TableName = "[OLAP_SEG4]";
            return TableName;
        }

    }
}