﻿namespace DynamicBoard.DataServices
{
    public class ChartDataset
    {
        public int Data { get; set; }

        public string Dataset_Label { get; set; }

        public string x_axis_labels { get; set; }

        public List<Dictionary<string, object>>? dataGrid { get; set; }
    }
}
