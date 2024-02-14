using DynamicBoard.Application.Model;
using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Services;
using DynamicBoard.Models;
using System.Data;

namespace DynamicBoard.Application.DomainServices
{
    public class ChartCommon
    {


        public static RenderChart ChartManipulation(ExtendDashboard extendDashboard, string chartType, string chartTitle, long chartID, string chartCSS, ExtendChart extendChart = null, string modifiedQuery = "")
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
                    datasetResult = db.DatasetExecute(dataScript, connesctionString);
                    //renderChart.RefershTime = extendDashboard.Charts.RefershTime;
                }
                if (extendChart != null)
                {
                    //var connesctionString = "Server=" + "10.38.38.199" + ";Database=" + extendChart.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendChart.DBConnections.User + ";Password=" + "SMedi@33333" + ";Integrated Security=False;";
                    var connesctionString = "Server=" + extendDashboard.DBConnections.Server + ";Database=" + extendDashboard.DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard.DBConnections.User + ";Password=" + extendDashboard.DBConnections.Password + ";Integrated Security=False;";

                    datasetResult = db.DatasetExecute(dataScript, connesctionString);
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
                            renderChart.jsonchartTitle =Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle.Replace("\r\n", ""));
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
                    var jsonchartTitle = Newtonsoft.Json.JsonConvert.SerializeObject(chartTitle);
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
