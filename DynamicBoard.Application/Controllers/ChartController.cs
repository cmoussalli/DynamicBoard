using ClosedXML.Excel;
using DynamicBoard.Application.DomainServices;
using DynamicBoard.Application.Model;
using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;

namespace DynamicBoard.Application.Controllers
{
    public class ChartController : BaseController
    {
        DynamicBoardChartServices dynamicBoardChartServices = new();
        DynamicBoardCommonServices dynamicBoardCommonServices = new();
        public async Task<ActionResult> Index()
        {

            int chartID = 1;
            List<ExtendChart> extendCharts = new List<ExtendChart>();
            RenderChart renderChart = new RenderChart();
            extendCharts = await dynamicBoardChartServices.ChartsGetByIdAsync(chartID);
            //var res = ViewBag.PortalModelData;
            //if (res.User.PreferedLanguageID == 1)
            //{
            //    renderChart = ChartCommon.ChartManipulation(null, extendCharts[0].ChartTypes.TitleEn, extendCharts[0].TitleAr, extendCharts[0].ID, "", extendCharts[0]);
            //}
            //else
            //{
            ExtendDashboard extendDashboard = new();
            extendDashboard.DBConnections = extendCharts[0].DBConnections;
            renderChart = await ChartCommon.ChartManipulation(extendDashboard, extendCharts[0].ChartTypes.TitleEn, extendCharts[0].TitleEn, extendCharts[0].ID, "", null, extendCharts[0]);
            //}
            // var json = Newtonsoft.Json.JsonConvert.SerializeObject(renderChart);
            return PartialView("ChartPv", renderChart);
            //return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChartNonEncrypted(long chartID, string parameters, string token)
        {
            if (chartID <= 0)
            {
                return PartialView("Error", "Parmeter chart Id not passed.");

            }
            else if (string.IsNullOrEmpty(parameters))
            {
                return PartialView("Error", "Parmeter (Parameters) not passed");
            }
            else if (string.IsNullOrEmpty(token))
            {
                return PartialView("Error", "Parmeter token not passed");
            }
            else
            {
                IActionResult result = await ChartView(chartID, parameters);
                return result;
            }

        }


        [HttpGet]
        public async Task<IActionResult> ChartTemplateView(int chartTypeID, long chartID)
        {
            RenderChart renderChart = new RenderChart();
            List<ChartDataset> chartDatasets = new();
            renderChart.ChartID = chartID;
            renderChart.IsAllowPrint = false;
            renderChart.IsAllowRefresh = false;
            ChartScriptTemplates chartScriptTemplates = await dynamicBoardChartServices.GetChartScriptTemplateByChartTypeIDAsync(chartTypeID);
            if (chartScriptTemplates != null)
            {


                List<Dataset> datasets = new();
                var result = EnumExtensions.ParseEnumValue<ChartType>(chartScriptTemplates.ID);
                //if (!result.EnumValue.ToString().Equals("DataGrid"))
                //{
                chartDatasets = await dynamicBoardCommonServices.GetChartDatasets(chartID, chartScriptTemplates.SQLScriptTemplate);
                //}
                //else
                //{

                //    chartDatasets = await db.GridDatasetExecute(chartID, chartScriptTemplates.SQLScriptTemplate);

                //}

                if (result.EnumValue.ToString() == "Label" || result.EnumValue.ToString() == "PieProgress")
                {
                    renderChart.ChartType = result.DisplayName;
                    renderChart.ChartID = chartID;
                    renderChart.json_graphConfigurations = "";
                    renderChart.JsonXaxis_labels = "";
                    renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartScriptTemplates.ChartTitle.Replace("\r\n", ""));
                    renderChart.LabelValue = chartDatasets.Select(a => a.Data).FirstOrDefault();
                    return View("ChartTemplateView", renderChart);
                }
                else if (result.EnumValue.ToString().Equals("DataGrid"))
                {
                    renderChart.ChartType = result.DisplayName;
                    renderChart.ChartID = chartID;
                    renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartScriptTemplates.ChartTitle.Replace("\r\n", ""));
                    return View("ChartTemplateView", renderChart);
                }
                else
                {
                    var DataArary = chartDatasets.Select(a => a.Data).ToArray();
                    var DatasetLabels = chartDatasets.Select(a => a.Dataset_Label.Replace("\r\n", "")).ToArray();
                    var TitleLabel = chartDatasets.Select(a => a.x_axis_labels).FirstOrDefault();
                    Dataset dataset = new();

                    dataset.label = chartScriptTemplates.ChartTitle;
                    dataset.data = DataArary.ToList();
                    datasets.Add(dataset);


                    var jsongraphConfigurations = Newtonsoft.Json.JsonConvert.SerializeObject(datasets);
                    var json_x_axis_labels = Newtonsoft.Json.JsonConvert.SerializeObject(DatasetLabels);
                    var jsonchartTitle = chartScriptTemplates.ChartTitle;// Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));

                    renderChart.ChartType = result.DisplayName;
                    renderChart.ChartID = chartID;
                    renderChart.json_graphConfigurations = jsongraphConfigurations;
                    renderChart.JsonXaxis_labels = json_x_axis_labels.ToString();
                    renderChart.jsonchartTitle = chartScriptTemplates.ChartTitle; ;

                    return View("ChartTemplateView", renderChart);
                }
            }
            else
            {
                return PartialView("EmptyChart");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetChartEncrypted(string data)
        {
            long chartId = 0;
            string parameters = "";
            string token = "";
            bool IsAllowRefersh = false;
            bool IsAllowPrint = false;
            int language = 0;
            if (string.IsNullOrEmpty(data))
            {
                return PartialView("Error", "Parmeter Data not passed.");
            }
            else
            {
                var DecryptedQueryString = EncryptionHelper.Decrypt(data);

                // Decrypted Data parsing
                string[] splitString = DecryptedQueryString.Split('&');

                // chartId 
                string[] partsForChartID = splitString[0].Split('=');
                chartId = Convert.ToInt64(partsForChartID[1]);
                // parameters
                string[] partsForParameters = splitString[1].Split('=');
                parameters = partsForParameters[1];
                // token
                string[] partsForToken = splitString[2].Split('=');
                token = partsForToken[1];
                // IsAllowRefersh
                string[] partsForIsAllowRefersh = splitString[3].Split('=');
                IsAllowRefersh = Convert.ToBoolean(partsForIsAllowRefersh[1]);
                //IsAllowPrint
                string[] partsForIsAllowPrint = splitString[4].Split('=');
                IsAllowPrint = Convert.ToBoolean(partsForIsAllowPrint[1]);

                // Language 2 for english and 1 for arabic
                string[] partsForLanguage = splitString[5].Split('=');
                language = Convert.ToInt32(partsForLanguage[1]);


                if (chartId <= 0)
                {
                    return PartialView("Error", "Parmeter chart Id not passed.");
                }
                else if (string.IsNullOrEmpty(parameters))
                {
                    return PartialView("Error", "Parmeter (parameters) not passed.");
                }
                else if (string.IsNullOrEmpty(token))
                {
                    return PartialView("Error", "Parmeter token not passed.");
                }

                else
                {
                    IActionResult result = await ChartView(chartId, parameters, IsAllowRefersh, IsAllowPrint, language);
                    return result;
                }

            }

        }



        [HttpGet]
        public async Task<IActionResult> GetRequestChartPV(long chartID, string parameters)
        {
            string modifiedQueryScript = "";
            List<ParamData> paramDataset = new List<ParamData>();
            List<ChartParameter> chartParameters = new();
            List<string> whereClauseList = new List<string>();
            List<ChartThemeExtends> chartThemes = new List<ChartThemeExtends>();
            //url = "/Chart/GetRequestChartPV?chartID=" + ChartId + "&parameters=[[CR:1000|2000][personID:222|333]";
            paramDataset = QueryStringParser(parameters);
            string paramDatasetValues = "";
            List<ExtendChart> extendCharts = new List<ExtendChart>();
            RenderChart renderChart = new RenderChart();
            extendCharts = await dynamicBoardChartServices.ChartsGetByIdAsync(chartID);
            chartParameters = await dynamicBoardCommonServices.GetChartParametersByChartID(chartID);
            chartThemes = await dynamicBoardChartServices.GetChartTheme(chartID);
            if (chartParameters != null)
            {
                if (chartParameters.Count > 0)
                {
                    // find the query string key 
                    foreach (var chartparms in chartParameters)

                    {
                        // chartParameters list from Database  ChartParameters
                        foreach (var item in paramDataset)
                        {
                            // check item (query string param exists in ChartParameters dataset )
                            var valueCheck = paramDataset.Where(a => a.Key == chartparms.Tag).FirstOrDefault();

                            if (valueCheck != null)
                            {
                                if (item.Key == chartparms.Tag)
                                {
                                    // If yes get the values of querystring 
                                    paramDatasetValues = string.Join(", ", paramDataset.Where(a => a.Key == item.Key).SelectMany(p => p.Values.Select(v => $"'{v}'")).ToList());
                                    if (!string.IsNullOrEmpty(paramDatasetValues))
                                    {
                                        // get the ChartParameters dataset SQLPlaceHolder;
                                        string placeholder = chartparms.SQLPlaceHolder;
                                        // and replace with query string values
                                        string replacePlaceholderParam = placeholder.Replace("[[" + item.Key + "]]", paramDatasetValues);
                                        whereClauseList.Add(replacePlaceholderParam);
                                    }
                                    paramDatasetValues = "";
                                    break;
                                }
                            }
                            else
                            {
                                if (chartparms.IsRequired)
                                {
                                    return PartialView("Error", chartparms.Tag + "are required in query string");
                                }
                                else
                                {

                                    // get the ChartParameters dataset SQLPlaceHolder;
                                    string placeholder = chartparms.SQLPlaceHolder;
                                    // and replace with default values
                                    string replacePlaceholderParam = placeholder.Replace("[[" + chartparms.Tag + "]]", chartparms.DefaultValue);
                                    paramDatasetValues = "";
                                    whereClauseList.Add(replacePlaceholderParam);
                                    break;
                                }

                            }
                        }
                    }

                    // when Querystring values added successfully
                    if (whereClauseList is not null)
                    {
                        if (whereClauseList.Count > 0)
                        {
                            if (extendCharts is not null)
                            {
                                if (extendCharts.Count > 0)
                                {
                                    // get the chart query script
                                    if (!string.IsNullOrEmpty(extendCharts[0].DataScript))
                                    {

                                        string mergeWhereClause = "";
                                        // extract the where clause from query script
                                        string whereClause = ExtractWhereClause(extendCharts[0].DataScript);
                                        if (!string.IsNullOrEmpty(whereClause))
                                        {
                                            // List to string
                                            var lsitconvertToString = string.Join(" ", whereClauseList);
                                            mergeWhereClause = whereClause + lsitconvertToString;
                                            modifiedQueryScript = extendCharts[0].DataScript;
                                            modifiedQueryScript = modifiedQueryScript.Replace(whereClause, mergeWhereClause);

                                        }
                                        else
                                        {
                                            // List to string
                                            var lsitconvertToString = string.Join(" ", whereClauseList);
                                            string rempveStartingAnd = lsitconvertToString.Replace("and ", string.Empty);
                                            rempveStartingAnd = "where " + rempveStartingAnd;
                                            modifiedQueryScript = extendCharts[0].DataScript;
                                            modifiedQueryScript = modifiedQueryScript.Replace("where", rempveStartingAnd);

                                        }

                                    }

                                }
                            }


                        }
                    }

                }
                else
                {
                    if (extendCharts is not null)
                    {
                        if (extendCharts.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(extendCharts[0].DataScript))
                            {
                                modifiedQueryScript = extendCharts[0].DataScript;

                            }
                            else
                            {
                                return PartialView("EmptyChart");
                            }

                        }
                        else
                        {
                            return PartialView("EmptyChart");
                        }
                    }

                }
            }


            try
            {
                if (extendCharts[0].ChartTypeID != 0)
                {
                    //{
                    ExtendDashboard extendDashboard = new();
                    extendDashboard.DBConnections = extendCharts[0].DBConnections;
                    renderChart = await ChartCommon.ChartManipulation(extendDashboard, extendCharts[0].ChartTypes.TitleEn, extendCharts[0].TitleEn, extendCharts[0].ID, "", chartThemes, extendCharts[0], modifiedQueryScript);

                    return PartialView("ChartPv", renderChart);
                }
                else
                {
                    return PartialView("EmptyChart");
                }
            }
            catch (Exception ex)
            {
                return PartialView("Error", "Verify your query script there is some issue on rendring chart. <b> Query Script are bellow <b>" + modifiedQueryScript);
            }

        }

        //public FileResult ExportToExcel(string requestedId, string civilNumber, string CRNo, string crTitle, string sectorID, string exhibitionDefinitionID, string status, bool isClosedRequests)
        //{

        //    using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
        //    {
        //        DataTable dt = new DataTable("Grid");

        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream()) //using System.IO;  
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exhibition.xlsx");
        //        }
        //    }
        //}
        [HttpPost]
        public ActionResult ExportToExcel(List<string>? Headers, List<Dictionary<string, string>>? Rows)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExportedData");

                //// Add Headers
                //for (int i = 0; i < tableData.Headers.Count; i++)
                //{
                //    worksheet.Cell(1, i + 1).Value = tableData.Headers[i];
                //    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                //    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                //}

                //// Add Rows
                //for (int rowIndex = 0; rowIndex < tableData.Rows.Count; rowIndex++)
                //{
                //    var row = tableData.Rows[rowIndex];
                //    for (int colIndex = 0; colIndex < tableData.Headers.Count; colIndex++)
                //    {
                //        string columnName = tableData.Headers[colIndex];
                //        worksheet.Cell(rowIndex + 2, colIndex + 1).Value = row[columnName];
                //    }
                //}

                // Auto-fit columns for better readability
                worksheet.Columns().AdjustToContents();

                // Save to MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
                }
            }
        }


        public class TableDataModel
        {
            public List<string>? Headers { get; set; }
            public List<Dictionary<string, string>>? Rows { get; set; }
        }




        [HttpGet]
        public async Task<IActionResult> ChartView(long chartID, string parameters, bool IsAllowRefresh = false, bool IsAllowPrint = false, int Language = 0)
        {
            string modifiedQueryScript = "";
            List<ParamData> paramDataset = new List<ParamData>();
            List<ChartParameter> chartParameters = new();
            List<string> whereClauseList = new List<string>();
            //url = "/Chart/GetRequestChartPV?chartID=" + ChartId + "&parameters=[[CR:1000|2000][personID:222|333]";
            paramDataset = QueryStringParser(parameters);
            string paramDatasetValues = "";
            List<ExtendChart> extendCharts = new List<ExtendChart>();
            RenderChart renderChart = new RenderChart();
            List<ChartThemeExtends> chartThemes = new List<ChartThemeExtends>();
            extendCharts = await dynamicBoardChartServices.ChartsGetByIdAsync(chartID);
            chartParameters = await dynamicBoardCommonServices.GetChartParametersByChartID(chartID);
            chartThemes = await dynamicBoardChartServices.GetChartTheme(chartID);
            string query = extendCharts[0].DataScript;
            if (chartParameters != null)
            {
                if (chartParameters.Count > 0)
                {
                    foreach (var chartparms in chartParameters)
                    {
                        //if (chartparms.IsRequired)
                        //{

                        if (!string.IsNullOrEmpty(query))
                        {
                            if (query.Contains(chartparms.Tag))
                            {
                                modifiedQueryScript = query;
                                // chartParameters list from Database  ChartParameters
                                var valueCheck = paramDataset.Where(a => a.Key == chartparms.Tag).FirstOrDefault();
                                if (valueCheck != null)
                                {
                                    if (valueCheck.Key == chartparms.Tag)
                                    {
                                        // If yes get the values of querystring 
                                        paramDatasetValues = string.Join(", ", paramDataset.Where(a => a.Key == valueCheck.Key).SelectMany(p => p.Values.Select(v => $"'{v}'")).ToList());
                                        if (!string.IsNullOrEmpty(paramDatasetValues))
                                        {
                                            // get tshe ChartParameters dataset SQLPlaceHolder;
                                            string placeholder = chartparms.SQLPlaceHolder;
                                            string replacePlaceholderParam = ReplacePlaceholder(placeholder, valueCheck.Key.Trim(), paramDatasetValues);

                                            if (replacePlaceholderParam.ToLower().Contains("in"))
                                            {
                                                var replacesinglequotes = replacePlaceholderParam.Replace("''", "'");
                                                replacePlaceholderParam = replacesinglequotes;
                                                query = query.Replace("[[" + chartparms.Tag + "]]", replacePlaceholderParam);

                                            }
                                            else
                                            {
                                                query = query.Replace("[[" + chartparms.Tag + "]]", replacePlaceholderParam);
                                            }

                                            modifiedQueryScript = query;
                                        }
                                        else
                                        {

                                            //modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", "");
                                            //query = modifiedQueryScript;
                                            // test
                                            if (chartparms.IsRequired)
                                            {
                                                // get tshe ChartParameters dataset SQLPlaceHolder;
                                                string placeholder = chartparms.SQLPlaceHolder;
                                                //modifiedQueryScript = query.Replace("[["+chartparms.Tag +"]]", placeholder);
                                                //modifiedQueryScript = query.Replace("[["+ chartparms.Tag +"]]", chartparms.DefaultValue);
                                                string replacePlaceholderParam = ReplacePlaceholder(placeholder, chartparms.Tag.Trim(), chartparms.DefaultValue);
                                                modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", replacePlaceholderParam);
                                                query = modifiedQueryScript;
                                            }
                                            else
                                            {
                                                if (query.Contains("=" + "[[" + chartparms.Tag + "]]"))
                                                {
                                                    modifiedQueryScript = query.Replace("=[[" + chartparms.Tag + "]]", "");

                                                }
                                                else
                                                {
                                                    modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", "");
                                                }
                                                query = modifiedQueryScript;
                                            }


                                        }

                                    }
                                }

                                else
                                {
                                    if (chartparms.IsRequired)
                                    {
                                        // get tshe ChartParameters dataset SQLPlaceHolder;
                                        string placeholder = chartparms.SQLPlaceHolder;
                                        //modifiedQueryScript = query.Replace("[["+chartparms.Tag +"]]", placeholder);
                                        //modifiedQueryScript = query.Replace("[["+ chartparms.Tag +"]]", chartparms.DefaultValue);
                                        string replacePlaceholderParam = ReplacePlaceholder(placeholder, chartparms.Tag.Trim(), chartparms.DefaultValue);
                                        modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", replacePlaceholderParam);
                                        query = modifiedQueryScript;
                                    }
                                    else
                                    {
                                        if (query.Contains("=" + "[[" + chartparms.Tag + "]]"))
                                        {
                                            modifiedQueryScript = query.Replace("=[[" + chartparms.Tag + "]]", "");

                                        }
                                        else
                                        {
                                            modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", "");
                                        }
                                        query = modifiedQueryScript;
                                    }
                                }
                            }
                        }
                        //}
                        //else
                        //{
                        //    modifiedQueryScript = query.Replace("[[" + chartparms.Tag + "]]", "");
                        //}
                    }


                }

            }


            try
            {
                if (extendCharts[0].ChartTypeID != 0)
                {
                    //{
                    ExtendDashboard extendDashboard = new();
                    extendDashboard.DBConnections = extendCharts[0].DBConnections;
                    if (string.IsNullOrEmpty(modifiedQueryScript))
                    {
                        modifiedQueryScript = extendCharts[0].DataScript;
                    }
                    var title = "";
                    if (Language == 2)
                    {
                        title = extendCharts[0].TitleEn;
                    }
                    else
                    {
                        title = extendCharts[0].TitleAr;
                    }

                    renderChart = await ChartCommon.ChartManipulation(extendDashboard, extendCharts[0].ChartTypes.TitleEn, title, extendCharts[0].ID, "", chartThemes, extendCharts[0], modifiedQueryScript);
                    renderChart.IsAllowRefresh = IsAllowRefresh;
                    renderChart.IsAllowPrint = IsAllowPrint;
                    if (renderChart.dataGrid != null && renderChart.dataGrid.Count > 0)
                    {
                        renderChart.IsAllowRefresh = false;
                        renderChart.IsAllowPrint = false;
                    }
                    return View("ChartView", renderChart);

                }
                else
                {
                    return PartialView("EmptyChart");
                }
            }
            catch (Exception ex)
            {
                return PartialView("Error", "Verify your query script there is some issue on rendring chart. <b> Query Script are bellow <b>" + modifiedQueryScript);
            }

        }
        static string ReplacePlaceholder(string input, string placeholder, string replacement)
        {
            string pattern = "\\[\\[" + Regex.Escape(placeholder) + "\\]\\]";
            return Regex.Replace(input, pattern, replacement);
        }
        static string ReplaceWhereClause(string query, string newWhereClause)
        {
            // Regular expression pattern to match the where clause
            string pattern = @"where\s+(.+?)(\s+group\s+by|$)";

            return Regex.Replace(query, pattern, "where " + newWhereClause.Trim() + "$2", RegexOptions.IgnoreCase);
        }
        static string ExtractWhereClause(string query)
        {
            // Regular expression pattern to match the where clause
            string pattern = @"where\s+(.+?)(\s+group\s+by|$)";

            Match match = Regex.Match(query, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            else
            {
                return ""; // No where clause found

            }
        }
        public List<ParamData> QueryStringParser(string parms)
        {
            List<ParamData> paramDataList = new List<ParamData>();
            ParamData param = new ParamData();
            List<string> clauseValues = new List<string>();

            string parmQueryString = parms;
            parmQueryString = parmQueryString.Trim('[', ']');

            string[] pairs = parmQueryString.Split("][", StringSplitOptions.RemoveEmptyEntries);
            if (pairs.Length > 0)
            {
                foreach (string pair in pairs)
                {
                    param = new();
                    string[] keyValue = pair.Split(':');
                    string key = keyValue[0];
                    if (!string.IsNullOrEmpty(key))
                    {
                        param.Key = key.Trim();
                    }
                    string[] splitedValues = keyValue[1].Split('|');
                    if (splitedValues.Length > 0)
                    {
                        foreach (var item in splitedValues)
                        {
                            if (!string.IsNullOrEmpty(item.Trim()))
                            {
                                clauseValues.Add(item.Trim());
                            }


                        }
                        param.Values = clauseValues;
                        clauseValues = new();
                    }

                    paramDataList.Add(param);
                }

            }
            return paramDataList;
        }

    }
}
