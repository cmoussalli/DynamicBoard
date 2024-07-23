namespace DynamicBoard.Application.Model
{
    public class RenderChart
    {
        public long ChartID { get; set; }
        public string ChartType { get; set; }
        public string JsonXaxis_labels { get; set; }
        public string jsonchartTitle { get; set; }
        public string json_graphConfigurations { get; set; }
        public string ChartCSS { get; set; }
        public int? RefershTime { get; set; }
        public bool IsAllowRefresh { get; set; }
        public bool IsAllowPrint { get; set; }
        public long LabelValue { get; set; }
        public int  Language { get; set; }
        public string Parameters { get; set; }
        public int ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public int SizeID { get; set; }
        public int SortID { get; set; }

    }

    public class RenderChartExtended:RenderChart
    {
        public RenderChartExtended()
        {
            RenderCharts = new List<RenderChart>();
        }
        public List<RenderChart> RenderCharts { get; set; }
      
        public int DashboardID { get; set; }
        public string DashboardTitle { get; set; }
        public string CRNumber { get; set; }
        public string Css { get; set; }
    }
}
