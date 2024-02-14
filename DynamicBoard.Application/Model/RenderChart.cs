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
        public long LabelValue { get; set; }
    }
}
