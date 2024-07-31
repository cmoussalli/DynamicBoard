using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices
{
    public class Lnk_Dashboards_Charts
    {
        public long ID { get; set; }

        public long DashboardID { get; set; }

        public long ChartID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int SortID { get; set; }
        public int SizeID { get; set; }

    }

    public class Lnk_Dashboards_Charts_Size_Extended : Lnk_Dashboards_Charts
    {
        public string Css { get; set; }
        public string ChartCSSTitle { get; set; }
    }

}