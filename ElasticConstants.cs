using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elastic.Controllers
{
    public class ElasticConstants
    {
        
        public static void SetDashboardElementProperties(IDictionary<string, object> Properties)
        {
            dynamic Settings = new System.Dynamic.ExpandoObject();
            Settings.legend = "0";
            Settings.showLabels = "1";
            Settings.showValues = "0";
            Settings.decimals = "0";
            Settings.sDecimals = "0";
            Settings.numberFormatting = "0";
            Settings.formatNumberScale = "1";
            Settings.sFormatNumberScale = "1";
            Settings.thousandSeparator = "1";
            Settings.bold = "0";
            Settings.italic = "0";
            Settings.underline = "0";
            Settings.fontFamily = "0";
            Settings.fontSize = "14";
            Settings.displayAs = "1";
            Settings.sDisplayAs = "1";
            Settings.textAlign = "0";
            Settings.color = "#000";
            Settings.bgColor = "#FFF";
            Settings.labelDisplay = "wrap";
            Properties.Add("Settings", Settings);
            Properties.Add("ChartType", "column");
            Properties.Add("y1Axis", "column");
            Properties.Add("y2Axis", "line");
        }

        public static string RemoveGuid(string Input)
        {
            string replaceString = "\"DashboardElementId\":\"00000000-0000-0000-0000-000000000000\",";
            Input = Input.Replace(replaceString, "");
            replaceString = "\"DashboardId\":\"00000000-0000-0000-0000-000000000000\",";
            Input = Input.Replace(replaceString, "");
            return Input;
        }
        public static void SetDashboardProperties(IDictionary<string, object> Properties)
        {
            string close = "{\r\n  \"lg\": [\r\n    {\r\n      \"w\": 78,\r\n      \"h\": 46,\r\n      \"x\": 0,\r\n      \"y\": 0,\r\n      \"i\": \"0\",\r\n      \"minW\": 6,\r\n      \"maxW\": 120,\r\n      \"minH\": 1,\r\n      \"maxH\": null,\r\n      \"moved\": false,\r\n      \"static\": false,\r\n      \"isDraggable\": true,\r\n      \"isResizable\": true\r\n    }\r\n  ],\r\n  \"md\": [\r\n    {\r\n      \"w\": 30,\r\n      \"h\": 12,\r\n      \"minW\": 6,\r\n      \"minH\": 1,\r\n      \"maxW\": 120,\r\n      \"maxH\": null,\r\n      \"x\": 0,\r\n      \"y\": null,\r\n      \"i\": \"0\",\r\n      \"isDraggable\": true,\r\n      \"isResizable\": true\r\n    }\r\n  ],\r\n  \"sm\": [\r\n    {\r\n      \"w\": 30,\r\n      \"h\": 12,\r\n      \"minW\": 6,\r\n      \"minH\": 1,\r\n      \"maxW\": 120,\r\n      \"maxH\": null,\r\n      \"x\": 0,\r\n      \"y\": null,\r\n      \"i\": \"0\",\r\n      \"isDraggable\": true,\r\n      \"isResizable\": true\r\n    }\r\n  ],\r\n  \"xxs\": [\r\n    {\r\n      \"w\": 30,\r\n      \"h\": 12,\r\n      \"x\": 0,\r\n      \"y\": 0,\r\n      \"i\": \"0\",\r\n      \"minW\": 6,\r\n      \"maxW\": 120,\r\n      \"minH\": 1,\r\n      \"maxH\": null,\r\n      \"moved\": false,\r\n      \"static\": false,\r\n      \"isDraggable\": true,\r\n      \"isResizable\": true\r\n    }\r\n  ]\r\n}";

            Properties.Add("Layout", close);
        }
    }
}