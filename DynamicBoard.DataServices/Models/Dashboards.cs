﻿namespace DynamicBoard.DataServices
{
    public class Dashboards
    {
        public long ID { get; set; }

        public string TitleEn { get; set; }

        public string TitleAr { get; set; }

        public Guid GUID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = "test user";
        public bool HideChartButtons { get; set; }
        public int ChartHeight { get; set; }
    }

}
