using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices
{
    public class Dashboard_Charts_Details
    {
        public long Lnk_Dashboard_Charts_ID { get; set; }
        public long DashboardID { get; set; }

        public long ChartID { get; set; }

        public string ChartTitleAr { get; set; }
        public string ChartTitleEn { get; set; }

        public string ChartTypeEn { get; set; }

        public string ChartTypeAr { get; set; }

        public int RefershTime { get; set; }
        public string DataScript { get; set; }

        public string Css { get; set; }

        public int SortID { get; set; }
        public int SizeID { get; set; }
    }

}