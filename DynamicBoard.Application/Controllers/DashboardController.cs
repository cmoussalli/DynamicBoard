using DynamicBoard.Application.DomainServices;
using DynamicBoard.Application.Model;
using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DynamicBoard.Application.Controllers
{
    public class DashboardController : BaseController
    {

        DynamicBoardDashboardServices dashboardServices = new();
        DynamicBoardCommonServices dynamicBoardCommonServices = new();
        DynamicBoardChartServices dynamicBoardChartServices = new();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardEncrypted(string encryptedParam)
        {
            int dashboardID = 0;
            long chartID = 0;
            //string parameters = "";
            string token = "";
            bool IsAllowRefersh = true;
            bool IsAllowPrint = true;
            string url = "";
            string parameters="";
            int language = 1;

            if (string.IsNullOrEmpty(encryptedParam))
            {
                return PartialView("Error", "Parmeter Data not passed.");
            }
            else
            {
                var DecryptedQueryString = EncryptionHelper.Decrypt(encryptedParam);

                // Decrypted Data parsing
                string[] splitString = DecryptedQueryString.Split('&');

                // chartId 
                string[] partsForDashboardID = splitString[0].Split('=');
                dashboardID = Convert.ToInt32(partsForDashboardID[1]);
                // parameters
                string[] partsForParameters = splitString[2].Split('=');
                parameters = partsForParameters[1];

                // token
                //string[] partsForToken = splitString[2].Split('=');
                //token = partsForToken[1];
                //// IsAllowRefersh
                //string[] partsForIsAllowRefersh = splitString[3].Split('=');
                //IsAllowRefersh = Convert.ToBoolean(partsForIsAllowRefersh[1]);
                ////IsAllowPrint
                //string[] partsForIsAllowPrint = splitString[4].Split('=');
                //IsAllowPrint = Convert.ToBoolean(partsForIsAllowPrint[1]);

                // Language 2 for english and 1 for arabic
                string[] partsForLanguage = splitString[1].Split('=');
                language = Convert.ToInt32(partsForLanguage[1]);
            }





            if (dashboardID == 0)
            {
                return PartialView("Error", "Invalid dashboard ID");
            }

            //Get dashboad by dashboardID
            Dashboards dashboard = await dashboardServices.GetDashboardByDashboardIDAsync(dashboardID);

            if (dashboard == null)
            {
                return PartialView("Error", "Dashboard Not Found.");
            }


            //Get charts by dashboardID
            List<Lnk_Dashboards_Charts_Size_Extended> lnk_Dashboards_Charts = await dashboardServices.GetLnkDashboardsChartsByDashboardIDAsync(dashboardID);
            //if (chartID > 0)
            //{
            //    lnk_Dashboards_Charts = lnk_Dashboards_Charts.Where(c => c.ChartID == chartID).ToList();
            //}

            if (lnk_Dashboards_Charts.Any())
            {
                List<RenderChart> renderCharts = await GenerateRenderChartData(lnk_Dashboards_Charts, chartID, dashboardID, language, parameters);
                string dashboardTitle = dashboard.TitleEn;
                if (language == 1)
                {
                    dashboardTitle = dashboard.TitleAr;
                }
                return await DashboardView(renderCharts, dashboardTitle, dashboardID, language, parameters);
            }
            else
            {
                return PartialView("Error", "Charts data not found.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard(int dashboardID=0,int chartID=0,int language=1, string parameters="")
        {
            long chartId = 0;
            //string parameters = "";
            string token = "";
            bool IsAllowRefersh = true;
            bool IsAllowPrint = true;
            string url = "";

            
            if (dashboardID==0)
            {
                return PartialView("Error", "Invalid dashboard ID");
            }

            //Get dashboad by dashboardID
            Dashboards dashboard  = await dashboardServices.GetDashboardByDashboardIDAsync(dashboardID);

            if (dashboard == null) {
                return PartialView("Error", "Dashboard Not Found.");
            }


            //Get charts by dashboardID
            List<Lnk_Dashboards_Charts_Size_Extended> lnk_Dashboards_Charts= await dashboardServices.GetLnkDashboardsChartsByDashboardIDAsync(dashboardID);
            if (chartID > 0) {
                lnk_Dashboards_Charts= lnk_Dashboards_Charts.Where(c => c.ChartID == chartID).ToList();
            }
            
            if(lnk_Dashboards_Charts.Any())
            {
                List<RenderChart> renderCharts = await GenerateRenderChartData(lnk_Dashboards_Charts, chartID, dashboardID, language,parameters);
                string dashboardTitle = dashboard.TitleEn;
                if (language == 1)
                {
                    dashboardTitle = dashboard.TitleAr;
                }
                return await DashboardView(renderCharts, dashboardTitle, dashboardID, language,parameters);
            }
            else
            {
                return PartialView("Error", "Charts data not found.");
            }

        }

        private async Task<List<RenderChart>> GenerateRenderChartData(List<Lnk_Dashboards_Charts_Size_Extended> lnk_Dashboards_Charts,long chartID,int dashboardID,int language,string parameters)
        {
            string url = "";
            var renderChartData = new RenderChart();
            List<RenderChart> renderCharts = new List<RenderChart>();
            foreach (Lnk_Dashboards_Charts_Size_Extended Chart in lnk_Dashboards_Charts)
            {
                List<ChartParameter> chartParameters = await dynamicBoardCommonServices.GetChartParametersByChartID(Chart.ChartID);
                List<ExtendChart> extendCharts = new List<ExtendChart>();

                extendCharts = await dynamicBoardChartServices.ChartsGetByIdAsync(Chart.ChartID);

                RenderChart renderChart = new RenderChart()
                {
                    ChartID = Chart.ChartID,
                    IsAllowPrint = true,
                    IsAllowRefresh = true,
                    Language = language,
                    Parameters = parameters,
                    SizeID=Chart.SizeID,
                    SortID=Chart.SortID,
                    ChartCSS=Chart.Css,
                    RefershTime= extendCharts[0].RefershTime
                };
                renderCharts.Add(renderChart);
            }
            return renderCharts;
        }

        public async Task<IActionResult> GetChartPartialView(long chartID,int dashboardID, int language,string parameters)
        {
            // var model = new MyModel { SomeProperty = "Updated Content" };

            List<Lnk_Dashboards_Charts_Size_Extended> lnk_Dashboards_Charts = await dashboardServices.GetLnkDashboardsChartsByDashboardIDAsync(dashboardID);
            if (chartID > 0)
            {
                lnk_Dashboards_Charts = lnk_Dashboards_Charts.Where(c => c.ChartID == chartID).ToList();
            }

            RenderChart renderChart = new RenderChart();
            List<RenderChart> renderCharts = await GenerateRenderChartData(lnk_Dashboards_Charts, chartID, dashboardID, language, parameters);
            RenderChartExtended renderChartExtended = await ProcessCharts(renderCharts, parameters);
            renderChartExtended.DashboardID=dashboardID;
            
            renderChart = renderChartExtended?.RenderCharts?.ToList().FirstOrDefault();
            return PartialView("_ChartPartial", renderChart);
        }


        [HttpGet]
        public async Task<IActionResult> DashboardView(List<RenderChart> renderCharts,string dashboardtitle,int dashboardID,int language,string parameters="")
        {
            
            RenderChartExtended renderChartExtended = await ProcessCharts(renderCharts,parameters);
            renderChartExtended.DashboardTitle = dashboardtitle;
            renderChartExtended.DashboardID = dashboardID;
            renderChartExtended.Language = language;
            renderChartExtended.Parameters = parameters;
            if (renderChartExtended != null)
            {
                if (renderChartExtended.ErrorType == 1)
                {
                   return PartialView("EmptyChart");
                }
                else if (renderChartExtended.ErrorType == 2)
                {
                    return PartialView("Error", "Verify your query script there is some issue on rendring chart. <b> Query Script are bellow <b>" + renderChartExtended.ErrorMessage);
                }
            }
            return View("DashboardView", renderChartExtended);
        }

        private async Task<RenderChartExtended> ProcessCharts(List<RenderChart> renderCharts, string parameters="")
        {
            string modifiedQueryScript = "";
            List<ParamData> paramDataset = new List<ParamData>();
            List<ChartParameter> chartParameters = new();
            List<string> whereClauseList = new List<string>();
            RenderChartExtended renderChartExtended = new RenderChartExtended();

            paramDataset = QueryStringParser(parameters);

            foreach (RenderChart chartItem in renderCharts)
            {

                string paramDatasetValues = "";
                List<ExtendChart> extendCharts = new List<ExtendChart>();

                List<ChartThemeExtends> chartThemes = new List<ChartThemeExtends>();
                extendCharts = await dynamicBoardChartServices.ChartsGetByIdAsync(chartItem.ChartID);
                chartParameters = await dynamicBoardCommonServices.GetChartParametersByChartID(chartItem.ChartID);
                chartThemes = await dynamicBoardChartServices.GetChartTheme(chartItem.ChartID);
                string query = extendCharts[0].DataScript;
                if (chartParameters != null)
                {
                    if (chartParameters.Count > 0 && extendCharts.Count > 0)
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
                    else
                    {
                        renderChartExtended.ErrorType = 1;
                        return renderChartExtended; //PartialView("EmptyChart");
                    }

                }
                else
                {
                    renderChartExtended.ErrorType = 1;
                    return renderChartExtended;
                }

                try
                {
                    if (extendCharts[0].ChartTypeID != 0)
                    {
                        ExtendDashboard extendDashboard = new();
                        extendDashboard.DBConnections = extendCharts[0].DBConnections;
                        if (string.IsNullOrEmpty(modifiedQueryScript))
                        {
                            modifiedQueryScript = extendCharts[0].DataScript;
                        }
                        var title = "";
                        if (chartItem.Language == 2)
                        {
                            title = extendCharts[0].TitleEn;
                        }
                        else
                        {
                            title = extendCharts[0].TitleAr;
                        }
                        RenderChart renderChart = await ChartCommon.ChartManipulation(extendDashboard, extendCharts[0].ChartTypes.TitleEn, title, extendCharts[0].ID, "", chartThemes, extendCharts[0], modifiedQueryScript);

                        renderChart.IsAllowRefresh = chartItem.IsAllowRefresh;
                        renderChart.IsAllowPrint = chartItem.IsAllowPrint;
                        renderChart.SizeID = chartItem.SizeID;
                        renderChart.SortID = chartItem.SortID;
                        renderChart.ChartCSS = chartItem.ChartCSS;
                        renderChart.RefershTime = extendCharts[0].RefershTime;
                        renderChartExtended.RenderCharts.Add(renderChart);
                    }
                    else
                    {
                        renderChartExtended.ErrorType = 1;
                        return renderChartExtended;
                    }

                }
                catch (Exception ex)
                {
                    renderChartExtended.ErrorType = 2;
                    renderChartExtended.ErrorMessage = modifiedQueryScript;
                    return renderChartExtended;
                    //return PartialView("Error", "Verify your query script there is some issue on rendring chart. <b> Query Script are bellow <b>" + modifiedQueryScript);
                }
            }
            return renderChartExtended;
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

    }
}
