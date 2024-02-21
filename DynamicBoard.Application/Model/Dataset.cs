namespace DynamicBoard.Application.Model
{
    public class Dataset
    {
        public string label { get; set; }
        public List<int> data { get; set; }
        public bool fill { get; set; }
        //public string borderColor { get; set; }
        public double tension { get; set; }
        public string x_axis_labels { get; set; }
        public int hoverOffset { get; set; }
    }
    public class ResultSet
    {
        public List<Dataset> datasets { get; set; }
    }
}
