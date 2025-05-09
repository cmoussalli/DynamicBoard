﻿using System.ComponentModel.DataAnnotations;

namespace DynamicBoard.DataServices.Models
{
    public class ChartScriptTemplates
    {
        public int ID { get; set; }
        public string SQLScriptTemplate { get; set; }
        public string ChartTitle { get; set; }

    }
    public enum ChartType
    {
        [Display(Name = "Line")]
        Line = 1,
        [Display(Name = "Bar")]
        Bar = 2,
        [Display(Name = "Pie")]
        Pie = 3,
        [Display(Name = "Stacked Bar")]
        StackedBar = 4,
        [Display(Name = "Column")]
        Column = 5,
        [Display(Name = "Label")]
        Label = 6,
        [Display(Name = "doughnut")]
        Doughnut = 7,
        [Display(Name = "polarArea")]
        PolarArea = 8,
        [Display(Name = "RadarChart")]
        RadarChart = 9,
        [Display(Name = "Horizontal Bar Chart")]
        HorizontalBarChart = 10,
        [Display(Name = "Progress Pie Chart")]
        PieProgress = 11,
        [Display(Name = "Data Grid")]
        DataGrid = 12
    }
}
