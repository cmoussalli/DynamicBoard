using DynamicBoard.Application.Model;
using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices.Services;
using DynamicBoard.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Data;

namespace DynamicBoard.Application.DomainServices
{
    public class ChartCommon
    {

        public static async Task<RenderChart> ChartManipulation(ExtendDashboard extendDashboard, string chartType, string chartTitle, long chartID, string chartCSS, List<ChartThemeExtends> chartThemes, ExtendChart extendChart = null, string modifiedQuery = "")
        {
            string dataScript = "";
            DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);
            List<int> data = new List<int>();

            RenderChart renderChart = new RenderChart();

            List<string> backgroundColor = new();
            List<string> borderColor = new();
            List<GraphConfiguration> graphConfigurationList = new List<GraphConfiguration>();
            GraphConfiguration graphConfiguration = new GraphConfiguration();
            Model.Dataset dataset = new();
            List<Model.Dataset> datasets = new();
            List<GraphDatasetDetails> graphList = new List<GraphDatasetDetails>();
            List<ChartDataset> datasetResult = null;
            List<Dictionary<string, object>> gridDatasetResult = null;
            try
            {
                if (!string.IsNullOrEmpty(modifiedQuery))
                {
                    dataScript = modifiedQuery;
                }
                else
                {
                    if (extendDashboard is not null)
                    {
                        dataScript = extendDashboard.Charts.DataScript;
                    }

                }
                if (chartType != "Data Grid")
                {


                    if (extendDashboard != null)
                    {
                        //var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
                        var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";
                        datasetResult = await db.DatasetExecute(dataScript, connesctionString);
                        //renderChart.RefershTime = extendDashboard.Charts.RefershTime;
                    }
                    if (extendChart != null)
                    {
                        //var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendChart.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendChart.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
                        var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";

                        datasetResult = await db.DatasetExecute(dataScript, connesctionString);
                        // renderChart.RefershTime = extendChart.RefershTime;
                    }
                }
                else
                {
                    var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";

                    gridDatasetResult = await db.GridDatasetExecute(dataScript, connesctionString);

                }
                if ((gridDatasetResult != null && gridDatasetResult.Count > 0) || (datasetResult != null && datasetResult.Count > 0))
                {
                    if (datasetResult?.Count > 0)
                    {
                        if (datasetResult[0].Dataset_Label is null) { datasetResult[0].Dataset_Label = ""; }
                        if (datasetResult[0].x_axis_labels is null) { datasetResult[0].x_axis_labels = ""; }
                    }
                    if (gridDatasetResult?.Count > 0 && datasetResult is null)
                    {
                        datasetResult = new List<ChartDataset>();
                        ChartDataset ds = new();
                        ds.dataGrid = gridDatasetResult;
                        renderChart.dataGrid = ds.dataGrid;
                    }
                    if (chartType == "Label")
                    {
                        renderChart.ChartType = chartType;
                        renderChart.ChartID = chartID;
                        renderChart.json_graphConfigurations = "";
                        renderChart.JsonXaxis_labels = "";
                        renderChart.ChartCSS = chartCSS;
                        renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
                        renderChart.LabelValue = datasetResult.Select(a => a.Data).FirstOrDefault();
                        return renderChart;
                    }
                    else if (chartType == "Progress Pie Chart")
                    {
                        renderChart.ChartType = chartType;
                        renderChart.ChartID = chartID;
                        renderChart.json_graphConfigurations = "";
                        renderChart.JsonXaxis_labels = "";
                        renderChart.ChartCSS = chartCSS;
                        renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
                        renderChart.LabelValue = datasetResult.Select(a => a.Data).FirstOrDefault();
                        return renderChart;
                    }
                    else if (chartType == "Data Grid")
                    {
                        renderChart.ChartType = chartType;
                        renderChart.ChartID = chartID;
                        renderChart.json_graphConfigurations = "";
                        renderChart.JsonXaxis_labels = "";
                        renderChart.ChartCSS = chartCSS;
                        renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
                        return renderChart;

                    }

                    var DataArary = datasetResult.Select(a => a.Data).ToArray();
                    var DatasetLabels = datasetResult.Select(a => a.Dataset_Label.Replace("\r\n", "")).ToArray();
                    var TitleLabel = datasetResult.Select(a => a.x_axis_labels).FirstOrDefault();
                    dataset.tension = 0.1;
                    dataset.hoverOffset = 10;

                    if (chartThemes is not null)
                    {
                        var countTheem = datasetResult.ToList().Count;
                        int loopRun = 0;
                        if (chartThemes.Count > 0)
                        {
                            //dataset.backgroundColor =
                            foreach (var item in chartThemes)
                            {
                                if (countTheem != loopRun)
                                {
                                    // FillBackgroundRGBA 
                                    backgroundColor.Add("rgba(" + item.ChartColor.FillBackgroundRGBA + ")");
                                    //FillBorderRGB
                                    borderColor.Add("rgb(" + item.ChartColor.FillBorderRGB + ")");

                                    loopRun++;
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }

                    }
                    dataset.label = chartTitle;
                    dataset.data = DataArary.ToList();
                    dataset.backgroundColor = backgroundColor;
                    dataset.borderColor = borderColor;
                    datasets.Add(dataset);


                    var datasetArray = datasets.ToArray();
                    var jsongraphConfigurations = Newtonsoft.Json.JsonConvert.SerializeObject(datasets);
                    var json_x_axis_labels = Newtonsoft.Json.JsonConvert.SerializeObject(DatasetLabels);
                    var jsonchartTitle = chartTitle.Replace("\r\n", "");// Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));

                    renderChart.ChartType = chartType;
                    renderChart.ChartID = chartID;
                    renderChart.json_graphConfigurations = jsongraphConfigurations;
                    renderChart.JsonXaxis_labels = json_x_axis_labels.ToString();
                    renderChart.jsonchartTitle = chartTitle;
                    renderChart.ChartCSS = chartCSS;
                }
                else
                {
                    renderChart.ChartType = chartType;
                    renderChart.ChartID = chartID;
                    renderChart.json_graphConfigurations = "";
                    renderChart.JsonXaxis_labels = "";
                    renderChart.jsonchartTitle = chartTitle;
                    renderChart.ChartCSS = chartCSS;
                }
            }
            catch (Exception ex)
            {


            }

            return renderChart;
        }


        public static async Task<RenderChart> ChartManipulation_Old(ExtendDashboard extendDashboard, string chartType, string chartTitle, long chartID, string chartCSS, ExtendChart extendChart = null, string modifiedQuery = "")
        {
            string dataScript = "";
            DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);
            List<int> data = new List<int>();

            RenderChart renderChart = new RenderChart();

            List<GraphConfiguration> graphConfigurationList = new List<GraphConfiguration>();
            GraphConfiguration graphConfiguration = new GraphConfiguration();
            DynamicBoard.Application.Model.Dataset dataset = new();
            List<DynamicBoard.Application.Model.Dataset> datasets = new();
            List<GraphDatasetDetails> graphList = new List<GraphDatasetDetails>();
            IEnumerable<DynamicBoard.DataServices.ChartDataset> datasetResult = null;

            try
            {
                if (!string.IsNullOrEmpty(modifiedQuery))
                {
                    dataScript = modifiedQuery;
                }
                else
                {
                    if (extendDashboard is not null)
                    {
                        dataScript = extendDashboard.Charts.DataScript;
                    }

                }
                if (extendDashboard != null)
                {
                    //var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
                    var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";
                    datasetResult = await db.DatasetExecute(dataScript, connesctionString);
                    //renderChart.RefershTime = extendDashboard.Charts.RefershTime;
                }
                if (extendChart != null)
                {
                    //var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendChart.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendChart.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
                    var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";

                    datasetResult = await db.DatasetExecute(dataScript, connesctionString);
                    // renderChart.RefershTime = extendChart.RefershTime;
                }


                if (datasetResult != null)
                {
                    foreach (var item in datasetResult)
                    {

                        if (chartType == "Label")
                        {
                            renderChart.ChartType = chartType;
                            renderChart.ChartID = chartID;
                            renderChart.json_graphConfigurations = "";
                            renderChart.JsonXaxis_labels = "";
                            renderChart.ChartCSS = chartCSS;
                            renderChart.jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
                            renderChart.LabelValue = item.Data;
                            return renderChart;
                        }
                        data = new List<int>();
                        dataset = new Model.Dataset();
                        //dataset.x_axis_labels = item.x_axis_labels;

                        dataset.label = item.Dataset_Label;
                        if (string.IsNullOrEmpty(item.x_axis_labels))
                        {
                            dataset.x_axis_labels = "Unspecified ";
                        }
                        else
                        {
                            dataset.x_axis_labels = item.x_axis_labels;

                        }

                        // datasets.Add(dataset);

                        dataset.tension = 0.1;
                        if (datasets.Count > 0)
                        {
                            Model.Dataset result = (Model.Dataset)datasets.Where(a => a.label == item.x_axis_labels).FirstOrDefault();
                            if (result is not null)
                            {
                                if (item.Dataset_Label == result.label)
                                {
                                    data.Add(item.Data);
                                    dataset.data = data;
                                    result.data.Add(item.Data);

                                }
                                else
                                {
                                    data.Add(item.Data);
                                    dataset.data = data;
                                    datasets.Add(dataset);
                                }
                            }
                            else
                            {

                                data.Add(item.Data);
                                dataset.data = data;
                                datasets.Add(dataset);
                            }
                        }
                        else
                        {
                            data = new List<int>();
                            data.Add(item.Data);
                            dataset.data = data;
                            datasets.Add(dataset);
                        }

                    }

                    // graphConfiguration.datasets = datasets;
                    //graphConfigurationList.Add(graphConfiguration);
                    var jsongraphConfigurations = Newtonsoft.Json.JsonConvert.SerializeObject(datasets);
                    var json_x_axis_labels = Newtonsoft.Json.JsonConvert.SerializeObject(datasets.Select(a => a.x_axis_labels).Distinct().ToList());
                    var jsonchartTitle = chartTitle.Replace("\r\n", "");// Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
                    renderChart.ChartType = chartType;
                    renderChart.ChartID = chartID;
                    renderChart.json_graphConfigurations = jsongraphConfigurations;
                    renderChart.JsonXaxis_labels = json_x_axis_labels.ToString();
                    renderChart.jsonchartTitle = chartTitle;
                    renderChart.ChartCSS = chartCSS;

                }
            }
            catch (Exception ex)
            {


            }

            return renderChart;
        }


        //public static RenderChart ChartManipulation_Old(ExtendDashboard extendDashboard, string chartType, string chartTitle, long chartID, string chartCSS, ExtendChart extendChart = null, string modifiedQuery = "")
        //{
        //    string dataScript = "";
        //    DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);
        //    List<int> data = new List<int>();

        //    RenderChart renderChart = new RenderChart();

        //    List<GraphConfiguration> graphConfigurationList = new List<GraphConfiguration>();
        //    GraphConfiguration graphConfiguration = new GraphConfiguration();
        //    Dataset dataset = new Dataset();
        //    List<Dataset> datasets = new List<Dataset>();
        //    List<GraphDatasetDetails> graphList = new List<GraphDatasetDetails>();
        //    IEnumerable<DynamicBoard.DataServices.ChartDataset> datasetResult = null;

        //    if (!string.IsNullOrEmpty(modifiedQuery))
        //    {
        //        dataScript = modifiedQuery;
        //    }
        //    else
        //    {
        //        dataScript = extendDashboard.Charts.DataScript;

        //    }
        //    if (extendDashboard != null)
        //    {
        //        //var connesctionString = "Server=" + extendDashboards[0].DBConnections.Server + ";Database=" + extendDashboards[0].DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + deserializedExtendDashboardJson[0].DBConnections.User + ";Password=" + deserializedExtendDashboardJson[0].DBConnections.Password + ";Integrated Security=False;";
        //        var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
        //        datasetResult = db.DatasetExecute(dataScript, connesctionString);
        //        renderChart.RefershTime = extendDashboard.Charts.RefershTime;
        //    }
        //    if (extendChart != null)
        //    {
        //        //var connesctionString = "Server=" + extendDashboards[0].DBConnections.Server + ";Database=" + extendDashboards[0].DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + deserializedExtendDashboardJson[0].DBConnections.User + ";Password=" + deserializedExtendDashboardJson[0].DBConnections.Password + ";Integrated Security=False;";
        //        var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendChart.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendChart.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
        //        datasetResult = db.DatasetExecute(dataScript, connesctionString);
        //        renderChart.RefershTime = extendChart.RefershTime;
        //    }


        //    if (datasetResult != null)
        //    {
        //        foreach (var item in datasetResult)
        //        {
        //            dataset = new Dataset();
        //            dataset.x_axis_labels = item.x_axis_labels;

        //            dataset.label = item.Dataset_Label;
        //            data = new List<int>();
        //            data.Add(item.Data);
        //            dataset.data = data;
        //            datasets.Add(dataset);
        //        }
        //        graphConfiguration.datasets = datasets;
        //        graphConfigurationList.Add(graphConfiguration);
        //        var jsongraphConfigurations = Newtonsoft.Json.JsonConvert.SerializeObject(graphConfigurationList);
        //        var json_x_axis_labels = Newtonsoft.Json.JsonConvert.SerializeObject(datasets.Select(a => a.x_axis_labels).Distinct().ToList());
        //        var jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle);
        //        renderChart.ChartType = chartType;
        //        renderChart.ChartID = chartID;
        //        renderChart.json_graphConfigurations = jsongraphConfigurations;
        //        renderChart.JsonXaxis_labels = json_x_axis_labels.ToString();
        //        renderChart.jsonchartTitle = chartTitle;
        //        renderChart.ChartCSS = chartCSS;

        //    }
        //    return renderChart;
        //}

    }
}
